(function ($) {
    var self = this;
    self.IsUpdate = false;    
    self.User = {
        id: null,
        full_name: null,
        password: "",
        email: "",
        address: "",
        phone_number: "",
        role: 0  
    }
    self.GetById = function (id, renderCallBack) {
        //self.userData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/AppUsers/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    ////Loading('hiden');
                },
                success: function (response) {
                    if (response.Data != null) {
                        //self.GetImageByProductId(id);
                        renderCallBack(response.Data);
                        self.Id = id;
                        
                    }
                }
            })
        }
    }

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Users/AddRegister',
            type: 'POST',
            dataType: 'json',
            data: {
                UserModelView: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {                   
                    window.location.href = '/';
                }
                else {
                    if (response.isPhoneExist) {
                        $(".phone_number-exist").show().text("Phone đã tồn tại");
                    }
                    if (response.isMailExist) {
                        $(".email-exist").show().text("Email đã tồn tại");
                    }
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/AppUsers/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                UserModelView: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
                }
               
            }
        })
    }


    self.ValidateLoginUser = function () {
        $("#form-login").validate({
            rules:
            {
                email_phone: {
                    required: true,
                },                
                password: {
                    required: true
                }
                
            },
            messages:
            {
                email_phone: {
                    required: "Email hoặc Số điện thoại không được trống",
                },               
                password: {
                    required: "Mật khẩu không được để trống",
                    min: "Mật khẩu ít nhất 6 kí tự"
                }
            },
            submitHandler: function (form) {

                var userName = $("#email_phone").val();
                var passWord = $("#password").val();
                self.AppUserLogin(userName, passWord);
            }
        });
    }


    self.AppUserLogin = function (userName,passWord) {
        $.ajax({
            url: '/Users/AppLogin',
            type: 'POST',
            dataType: 'json',
            data: {
                userName: userName,
                passWord: passWord
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    window.location.href = '/';
                }
                else {
                    alert("Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
        })
    }




    self.ValidateUser = function () {
        $("#form-submit").validate({
            rules:
            {
                full_name: {
                    required: true,
                },
                phone_number: {
                    required: true,
                    //headphone: false,
                    minlength: 10,
                    min: 0,
                    maxlength: 10,
                    number: isNaN
                },
                password: {
                    required: true
                },
                email: {
                    required: true
                },
                confirm: {
                    required: true,
                    equalTo: "#confirm_password"
                }//,
                //confirm: {
                //    required: "Bạn chưa nhập lại mật khẩu.",
                //    equalTo: "Mật khẩu không đúng.",
                //    min: "Mật khẩu ít nhất 8 kí tự"
                //}
            },
            messages:
            {
                full_name: {
                    required: "Họ tên không được để trống",
                },
                phone_number: {
                    required: "Số điện thoại không được để trống",
                    headphone: "Số điện thoại không hợp lệ"
                },
                password: {
                    required: "Mật khẩu không được để trống",
                    min: "Mật khẩu ít nhất 6 kí tự"
                },
                email: {
                    required: "Email không được để trống"
                },
                confirm: {
                    required: "Bạn chưa nhập lại mật khẩu.",
                    equalTo: "Mật khẩu không đúng.",
                    min: "Mật khẩu ít nhất 8 kí tự"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
               
                self.AddUser(self.User);
            }
        });
    }

    self.GetValue = function () {
        self.User.full_name = $("#full_name").val();
        self.User.phone_number = $("#phone_number").val();
        self.User.email = $("#email").val();
        self.User.address = $("#address").val();
        self.User.role = $("#role").val(); 
        self.User.password = $("#password").val();      

    }


    self.RenderHtmlByObject = function (view) {
        $("#full_name").val(view.full_name);
        $("#phone_number").val(view.phone_number);
        $("#email").val(view.email);
        $("#address").val(view.address);
        $("#role").val(view.role);
        $("#password").val(view.password);
    }


    $(document).ready(function () {
        self.ValidateUser();
        self.ValidateLoginUser();
    })
})(jQuery);