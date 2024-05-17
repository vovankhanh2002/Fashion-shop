var categories = "categories"
var company = "company"
var navbar = "navbar"
var product = "product"
var oderheader = "oderheader"
var oderdetails = "oderDetails"
var applicationuser = "applicationuser"
var newsletter = "newsletter"
var contact = "contact"
var reviews = "reviews"
var slideimage = "slideimage"
$(document).ready(function(){
    loadDatatable();
    Categories();
    Navbar();
    Company();
    Oderheader();
    Oderdetails();
    Application();
    Newsletter();
    Contact();
    Slideimage()
})
// Product
function loadDatatable() {
    new DataTable('#example',{
        "ajax": {
            "url":"/Admin/product/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Mã sản phẩm', "data": "id", "width": "15%" },
            { title: 'Tên sản phẩm', "data": "title", "width": "15%" },
            {
                title: 'Hình',
                "data": "imageUrl"
                , "render": function (data) {
                    return `
                        <div>
                            <img style="width:100px" src="/Content/assets/img/Product/${data}"></img>
                        </div>
                        
                    `
                }
                , "width": "15%"

            },
            { title: 'Mô tả',"data": "description", "width": "15%" },
            { title: 'Giá',"data": "price", "width": "15%" },
            { title: 'Lượt xem', "data": "views", "width": "15%" },
            { title: 'Tồn kho', "data": "warehouse", "width": "15%" },
            { title: 'Giá giảm', "data": "pricetotal", "width": "15%" },
            { title: 'Màu', "data": "color", "width": "15%" },
            { title: 'Kích thước', "data": "size", "width": "15%" },
            { title: 'Hoạt động', "data": "active", "width": "15%" },
            { title: 'Loại', "data": "categories.nameCategori", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div style= "width: 300px ">
                            <a href="/admin/product/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${product})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>
                            </a>
                            <a href="/admin/Slideimage/Create?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-plus"></i>
                            </a>
                        </div>
                        
                    `
                }                  
                ,"width": "20%" }
        ]
    })
}
// Product
function Slideimage() {
    new DataTable('#Slideimage', {
        "ajax": {
            "url": "/Admin/Slideimage/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên sản phẩm', "data": "product.title", "width": "15%" },
            {
                title: 'Hình slide',
                "data": "image"
                , "render": function (data) {
                    return `
                        <div>
                            <img style="width:100px" src="/Content/assets/img/sideimage/${data}"></img>
                        </div>
                        
                    `
                }
                , "width": "15%"

            },
            {
                title: 'Hình',
                "data": "product.imageUrl"
                , "render": function (data) {
                    return `
                        <div>
                            <img style="width:100px" src="/Content/assets/img/product/${data}"></img>
                        </div>
                        
                    `
                }
                , "width": "15%"

            },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div style= "width: 300px ">
                            <a href="/admin/Slideimage/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${slideimage})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>
                            </a>
                        </div>
                        
                    `
                }
                , "width": "20%"
            }
        ]
    })
}
// Categories
function Categories() {
    new DataTable('#Categories', {
        "ajax": {
            "url": "/Admin/Categories/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên loại', "data": "nameCategori", "width": "15%" },
            {
                title: 'Hình', "data": "ImageUrl"
                , "render": function (data) {
                    return `
                        <img src="${data}">
                    `
                }
                ,"width": "15%"
            },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div>
                            
                            <a href="/admin/categories/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${categories})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Navbar
function Navbar() {
    new DataTable('#Navbar', {
        "ajax": {
            "url": "/Admin/Navbar/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên thanh điều hướng', "data": "titleNavBar", "width": "15%" },
            { title: 'Địa chỉ ', "data": "urlNavBar", "width": "15%" },
            { title: 'Ngày đặt', "data": "dateSetNavbar", "width": "15%" },
            { title: 'Ngày xóa', "data": "dateOutNavbar", "width": "15%" },

            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div>
                            <a href="/admin/navbar/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${navbar})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Company
function Company() {
    new DataTable('#Company', {
        "ajax": {
            "url": "/Admin/Company/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên', "data": "name", "width": "15%" },
            { title: 'Địa chỉ đường phố', "data": "streetAddress", "width": "15%" },
            { title: 'Thành phố', "data": "city", "width": "15%" },
            { title: 'Tình trạng', "data": "state", "width": "15%" },
            { title: 'Mã bưu điện', "data": "postalCode", "width": "15%" },
            { title: 'Số điện thoại', "data": "phoneNumber", "width": "15%" },
            { title: 'Ngày đặt', "data": "dateTime", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div style= "width: 300px ">
                            <a href="/admin/Company/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${company})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// OderHeader
function Oderheader() {
    new DataTable('#Oderheader', {
        "ajax": {
            "url": "/Admin/OderHeader/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Mã đơn hàng', "data": "id", "width": "5%" },
            { title: 'Mã người dùng', "data": "applicationUserId", "width": "5%" },
            { title: 'Họ', "data": "lastName", "width": "5%" },
            { title: 'Tên', "data": "firstName", "width": "5%" },
            { title: 'Email', "data": "email", "width": "5%" },
            { title: 'Địa chỉ đường phố', "data": "streetAddress", "width": "5%" },
            { title: 'Tình trạng', "data": "state", "width": "5%" },
            { title: 'Mã bưu điện', "data": "postalCode", "width": "5%" },
            { title: 'Số điện thoại', "data": "phoneNumber", "width": "5%" },
            {
                title: 'Trạng thái đơn hàng',
                "data": "oderStatus",
                "render": function (data) {
                    if (data == "Pending") {
                        return `<p style="color:green">Chờ xác nhận đơn hàng</p>`
                    } else if (data == "Approved") {
                        return `<p style="color:FFD333">Đã lấy hàng</p>`
                    } else if (data == "Processing") {
                        return `<p style="color:forestgreen">Đã gửi đi</p>`
                    } else if (data == "Shipped") {
                        return `<p style="color:blue">Đang vận chuyển</p>`
                    } else if (data == "Success") {
                        return `<p style="color:greenyellow">Đã giao</p>`
                    }
                    else {
                        return `<p style="color:red">Đơn hàng đã hủy</p>`

                    }
                },
                "width": "5%"

            },
            {
                title: 'Trạng thái thanh toán',
                "data": "paymentStatus",
                "render": function (data) {
                    if (data == "Pending") {
                        return `<p style="color:green">Pending</p>`
                    } else if (data == "Approved") {
                        return `<p style="color:FFD333">Approved</p>`
                    } else if (data == "PaymentStatusDelayPayment") {
                        return `<p style="color:blue">PaymentStatusDelayPayment</p>`
                    } else {
                        return `<p style="color:red">Cancelled</p>`

                    }
                },
                "width": "5%"

            },

            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div style= "width: 300px ">
                            <a onclick="getByIdDetail(${data})" class="btn btn btn-info" >
                                <i class="fa-regular fa-eye"></i>
                            </a>
                            <a onclick="deleteAl(${data},${oderheader})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>
                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Oderdetails
function Oderdetails() {
    new DataTable('#Oderdetails', {
        "ajax": {
            "url": "/Admin/OderDetails/Load"
        }, function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Mã đơn hàng', "data": "oderHeaderId", "width": "15%" },
            { title: 'Mã sản phẩm', "data": "productId", "width": "15%" },
            { title: 'Số lượng', "data": "count", "width": "15%" },
            { title: 'Giá sản phẩm', "data": "price", "width": "15%" },
            { title: 'Tên sản phẩm', "data": "tittle", "width": "15%" },
            { title: 'Màu', "data": "color", "width": "15%" },
            { title: 'Kích thước', "data": "size", "width": "15%" },
        ]
    })
}
// ApplicationUser
function Application() {
    new DataTable('#Application', {
        "ajax": {
            "url": "/Admin/ApplicationUser/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên người dùng', "data": "userName", "width": "5%" },
            { title: 'Sô điện thoại', "data": "phoneNumber", "width": "5%" },
            { title: 'Khóa cuối', "data": "lockoutEnd", "width": "5%" },
            { title: 'Khóa bật', "data": "lockoutEnabled", "width": "5%" },
            { title: 'Thành phố', "data": "city", "width": "5%" },
            { title: 'Tên', "data": "name", "width": "5%" },
            { title: 'Tình trạng', "data": "state", "width": "5%" },
            { title: 'Địa chỉ đường phố', "data": "streetAddress", "width": "5%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div style= "width: 300px ">
                            <a href="/admin/ApplicationUser/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a href="/admin/ApplicationUser/delete?id=${data}" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>
                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Newsletter
function Newsletter() {
    new DataTable('#Newsletter', {
        "ajax": {
            "url": "/Admin/newsletter/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Địa chỉ email', "data": "email", "width": "15%" },
            { title: 'Tài khoản', "data": "applicationUser.userName", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div>
                            <a onclick="deleteAl(${data},${newsletter})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Navbar
function Contact() {
    new DataTable('#Contact', {
        "ajax": {
            "url": "/Admin/Contact/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên', "data": "name", "width": "15%" },
            { title: 'Email', "data": "email", "width": "15%" },
            { title: 'Chủ đề', "data": "subject", "width": "15%" },
            { title: 'Tin nhắn', "data": "message", "width": "15%" },

            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div>
                            <a href="/admin/contact/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${contact})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
// Review
function Review() {
    new DataTable('#Reviews', {
        "ajax": {
            "url": "/Admin/Reviews/Load"
        },
        function() {
            this.api()
                .columns()
                .every(function () {
                    let column = this;
                    let title = column.footer().textContent;

                    // Create input element
                    let input = document.createElement('input');
                    input.placeholder = title;
                    column.footer().replaceChildren(input);

                    // Event listener for user input
                    input.addEventListener('keyup', () => {
                        if (column.search() !== this.value) {
                            column.search(input.value).draw();
                        }
                    });
                });
        },
        "columns": [
            { title: 'Tên', "data": "name", "width": "15%" },
            { title: 'Email', "data": "email", "width": "15%" },
            { title: 'Nội dung', "data": "messageReview", "width": "15%" },
            { title: 'Sao', "data": "star", "width": "15%" },
            { title: 'Thời gian', "data": "dateTime", "width": "15%" },
            { title: 'Thông tin khách hàng', "data": "applicationUser.name", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                        <div>
                            <a href="/admin/reviews/edit?id=${data}" class="btn btn-primary">
                                <i class="fa-solid fa-user-pen"></i>
                            </a>
                            <a onclick="deleteAl(${data},${reviews})" class="btn btn-danger">
                                <i class="fa-solid fa-trash-can"></i>

                            </a>
                        </div>
                        
                    `
                }
                , "width": "15%"
            }
        ]
    })
}
function deleteAl(data, name) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: 'https://localhost:7043/admin/'+name+'/delete',
                method: 'POST',
                data: { id: data },
                success: (result) => {
                    if (result.success == true) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                        function load() {
                            window.location.reload();
                        }
                        window.setTimeout(load, 2000);
                        
                    }
                }
            })
        }
    })
}

function getByIdDetail(data) {
    $.ajax({
        url: '/admin/OderHeader/getByIdDetailSuccess',
        method: 'POST',
        data: { id: data },
        success: (result) => {
            if (result.success == true) {
                function load() {
                    window.location.href = "/admin/OderHeader/getByIdDetail/" + data
                }
                window.setTimeout(load, 1000);

            }
        }
    })
}