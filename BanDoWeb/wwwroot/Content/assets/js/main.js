(function ($) {
    "use strict";
    $(".chat_on").click(function () {
        $(".Layout").toggle();
        $(".chat_on").hide(300);
    });

    $(".chat_close_icon").click(function () {
        $(".Layout").hide();
        $(".chat_on").show(300);
    });

    // Dropdown on mouse hover
    $(document).ready(function () {
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod();

        $(window).resize(toggleNavbarMethod);
    });
    
    
    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Vendor carousel
    $('.vendor-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0:{
                items:2
            },
            576:{
                items:3
            },
            768:{
                items:4
            },
            992:{
                items:5
            },
            1200:{
                items:6
            }
        }
    });


    // Related carousel
    $('.related-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:2
            },
            768:{
                items:3
            },
            992:{
                items:4
            }
        }
    });

    
    var sumTotal = 0
    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        button.parent().parent().find('input').val(newVal);
        var id = $(this).data('id')
        var price = $('#price_' + id).data('price')
        var sum = price * newVal
        $('#Total_' + id).html(sum)
        $('#input_' + id).val(sum)
        $.ajax({
            url: '/Cart/updatecart',
            type: 'POST',
            data: { id: id, quantity: newVal },
            success: function (rs) {
                if (rs.success) {
                    //window.location.reload();
                    var sumTotal = 0
                    var array = $.makeArray($('tbody tr[id] td input[id]').map(function () {
                        return this.id;
                    }));
                    for (var i = 0; i < array.length; i++) {
                        i += 1
                        var total = $('#' + array[i]).val()
                        sumTotal += parseInt(total)
                    }
                    $('#Subtotal').html(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total))
                    var sumTa = sumTotal + 10
                    $('#Total').html(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(sumTa))

                }
            }
        });
        
    });
    
})(jQuery);
