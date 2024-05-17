window.addEventListener("load", () => {
    if (document.getElementById("qrCodeData").getAttribute('data-url') != null) {
        const uri = document.getElementById("qrCodeData").getAttribute('data-url');
        new QRCode(document.getElementById("qrCode"),
            {
                text: uri,
                width: 150,
                height: 150
            });
    }
});