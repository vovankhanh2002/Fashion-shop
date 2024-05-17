$(document).ready(function () {
    $('#btnChecked').click(function () {
        Checked()
    }) 
})

function Checked() {
    var arrCategori = new Array();
    var arrPrice = new Array();
    var arrColor = new Array();
    var arrSize = new Array();

    var Categori = document.getElementsByName('Categori');
    var Color = document.getElementsByName('Color');
    var Size = document.getElementsByName('Size');
    var Price = document.getElementsByName('Price');
    for (var i = 0, length = Categori.length; i < length; i++) {
        if (Categori[i].checked) {
            arrCategori.push(Categori[i].value);
        }
    }
    for (var i = 0, length = Color.length; i < length; i++) {
        if (Color[i].checked) {
            arrColor.push(Color[i].value);
        }
    }
    for (var i = 0, length = Size.length; i < length; i++) {
        if (Size[i].checked) {
            arrSize.push(Size[i].value);
        }
    } for (var i = 0, length = Price.length; i < length; i++) {
        if (Price[i].checked) {
            arrPrice.push(Price[i].value);
        }
    }
    /*$("input[name=Categori]:checked").each(function () {
        arrCategori.push($(this).val());
    });
    for (var i = 0; i < arrPrice.length; i++) {
        console.log(arrPrice[i])
    }   
    $("input[name=Price]:checked").each(function () {
        arrPrice.push($(this).val());
    });
    for (var i = 0; i < arrPrice.length; i++) {
        console.log(arrPrice[i])
    }   
    
    $("input[name=Color]:checked").each(function () {
        arrColor.push($(this).val());
    });
    for (var i = 0; i < arrColor.length; i++) {
        console.log(arrPrice[i])
    }  

    $("input[name=Size]:checked").each(function () {
        arrSize.push($(this).val());
    });
    for (var i = 0; i < arrSize.length; i++) {
        console.log(arrSize[i])
    }  
    */
    $.ajax({
        method:"POST",
        data: {
            lstCategori: arrCategori,
            lstPrice: arrPrice,
            lstColor: arrColor,
            lstSize: arrSize
        },
        url: "/shop/Filter",
        success: function (data) {
            var lstProduct = data
            var strProduct = ""
            console.log(lstProduct.data[0])
            for (var i = 0; i < lstProduct.data.length; i++) {
                console.log(lstProduct.data[i].id)
                strProduct += `<div class="col-lg-4 col-md-6 col-sm-6 pb-1 w-300">`
                    strProduct += `<div class="product-item bg-light">`
                        strProduct += `<div class="product-img position-relative overflow-hidden">`
                            strProduct += `<img class="img-fluid w-100" src="/Content/assets/img/Product/${lstProduct.data[i].imageUrl}" alt="">`
                            strProduct += `<div class="product-action">`
                                    strProduct += `<a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-shopping-cart"></i></a>`
                strProduct += `<a class="btn btn-outline-dark btn-square" href="/Detail/index/${lstProduct.data[i].id}"><i class="fa fa-search"></i></a>`
                            strProduct += `</div>`
                        strProduct += `</div>`
                    strProduct += `<div class="text-center py-4">`
                strProduct += `<a class="h6 text-decoration-none font-weight-normal" href="/detail/${lstProduct.data[i].id}">${lstProduct.data[i].title}</a>`
                    strProduct += `<div class="d-flex align-items-center justify-content-center mt-2">`
                strProduct += `<h5 class="font-weight-normal" style="color:red">${new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(lstProduct.data[i].price)}</h5>`
                strProduct += `<h6 class="text-muted ml-2" font-weight-normal><del>${new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(lstProduct.data[i].priceTotal) }</del></h6>`
                strProduct += `</div>`
                    strProduct += `</div>`
                    strProduct += `</div >`
                strProduct += `</div >`
            }
            $('#showProduct').html(strProduct)
        }
    })
    
}
//
/*function Show() {
    var getIdShow = $('#showProduct').attr('id')
    $.ajax({
        url: "/shop/Filter",
        method: "POST",
        success: function (data) {
            var lstProduct = data
            var strProduct = ""
            for (var i = 0; i < lstProduct.count; i++) {
                console.log(data.lstProduct[i].Id)
                strProduct += `"<div class="col - lg - 4 col - md - 6 col - sm - 6 pb - 1">`
                strProduct  +=  `"< div class="product-item bg-light mb-4" >"`
                strProduct += `"<div class="product-img position-relative overflow-hidden">"`
                strProduct += `"<img class="img-fluid w-100" src="~/Content//assets/img/Product/${data.lstProduct[i].Id}" alt="">"`
                strProduct  += `"<div class="product-action">"`
                strProduct  += `"<a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-shopping-cart"></i></a>"`
                strProduct  += `"<a class="btn btn-outline-dark btn-square" href=""><i class="far fa-heart"></i></a>"`
                strProduct  += `"<a class="btn btn-outline-dark btn-square" href=""><i class="fa fa-sync-alt"></i></a>"`
                strProduct += `"<a class="btn btn-outline-dark btn-square" asp-action="Detail" asp-controller="Detail" asp-route-id="${data.lstProduct[i].Id}"><i class="fa fa-search"></i></a>"`
                strProduct  += `"</div>"`
                strProduct  += `"</div>"`
                strProduct  += `"<div class="text-center py-4">"`
                strProduct += `"<a class="h6 text-decoration-none text-truncate" href="">${data.lstProduct[i].Title}</a>"`
                strProduct  += `"<div class="d-flex align-items-center justify-content-center mt-2">"`
                strProduct += `"<h5>$@item.Price</h5><h6 class="text-muted ml-2"><del>${data.lstProduct[i].totalPrice}</del></h6>"`
                strProduct  += `"</div>"`
                strProduct  += `"<div class="d-flex align-items-center justify-content-center mb-1">"`
                strProduct  += `"<small class="fa fa-star text-primary mr-1"></small>"`
                strProduct  += `"<small class="fa fa-star text-primary mr-1"></small>"`
                strProduct  += `"<small class="fa fa-star text-primary mr-1"></small>"`
                strProduct  += `"<small class="fa fa-star text-primary mr-1"></small>"`
                strProduct  += `"<small class="fa fa-star text-primary mr-1"></small>"`
                strProduct += `"<small>(${data.lstProduct[i].Views})</small>"`
                strProduct  += `"</div>"`
                strProduct  += `"</div>"`
                strProduct  += `"</div >"`
                strProduct  += `"</div >"`
            }
            getIdShow.html(strProduct)
        }
    });
}*/
