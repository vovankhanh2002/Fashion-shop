$(document).ready(function () {
    
    $('#AddtoCart').click(function () {
        formCart()
    })
    $('body').on('click', '#btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa sản phẩm khỏi giỏ hàng')
        if (conf == true) {
            $.ajax({
                url: '/Cart/DeleteCart',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.success) {
                        $('#trow_' + id).remove()
                        window.location.reload();
                    }
                }
            });
        }

    });
    $('.quantitys button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-pluss')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });
})

function formCart() {
    
    var arrColor = new Array()
    var arrSize = new Array()
    var productId = $('#productId').val()
    var quantity = $('#Quantity').val()

    $("input[name=size]:checked").each(function () {
        arrSize.push($(this).val());
    });
    
    $("input[name=color]:checked").each(function () {
        arrColor.push($(this).val());
    });
    
    if (arrColor.length > 0 && arrSize.length > 0 ) {
        $.ajax({
            method: "POST",
            data: {
                id: productId,
                quantity: quantity,
                lstColor: arrColor,
                lstSize: arrSize
            },
            url: "/cart/AddToCart",
            success: function (res) {
                if (res.success == false) {
                    location.href = "https://localhost:7043/Identity/Account/Login?returnUrl=" + window.location.pathname;
                }else if (res.bassQuantity == true) {
                    Swal.fire('The quantity of products in stock is not enough!')
                }else {
                    alert("Bạn đã thêm một sản phẩm vào giỏ hàng!")
                    window.location.reload()
                }
            }
        })
    } else {
        alert("Bạn chưa chọn màu và size")
    }
    
}
