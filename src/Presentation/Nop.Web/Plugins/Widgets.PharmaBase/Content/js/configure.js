
function sendMarketPlace() {
  $.ajax({
    url: '/MarketPlaceData/SendAuthInfo',
    type: 'POST',
    data: {},
    success: function (result) {
      //alert("Bilgileriniz başarılı bir şekilde kayıt edildi.");
      console.log("işlem başarılı");
    },
    error: function (xhr, status, error) {
      //alert("Bilgileriniz kaydedilirken bir hata ile karşılaşıldı. Lütfen tekrar deneyiniz...")
      console.log("işlem başarısız");
    }
  });
}



document.getElementById('sendMarketPlaceBtn').addEventListener('click', function () {
  // Konfirmasyon mesajı göster
  var userConfirmed = confirm("Mevcut ürünleri göndermek üzeresiniz. Onaylıyor musunuz?");

  if (userConfirmed) {
    alert("Ürünleri gönderme işlemi biraz uzun sürebilir. Lütfen bekleyiniz.")
    // Kullanıcı onayladıysa, AJAX işlemini başlat
    sendMarket();
  } else {
    // Kullanıcı iptal ettiyse, hiçbir şey yapma
    console.log("İşlem kullanıcı tarafından iptal edildi.");
  }
});

function sendMarket() {
  $.ajax({
    url: '/SendProduct/SendProducts',
    type: 'POST', 
    data: {  },
    success: function (result) {
      alert("Ürünler başarıyla gönderildi.")
      console.log("işlem başarılı");
    },
    error: function (xhr, status, error) {
      alert("Ürünler gönderilirken bir hata oluştur.")
      console.log("işlem başarısız");
    }
  });
}

function sendCategory() {
  // Seçili değeri almak için:
  var selectedCategoryId = document.getElementById("ParentCategoryId").value;
  var selectedCategory2 = document.getElementById("TrendYolCategoryId").value;
  var x = parseInt(selectedCategoryId);
  var y = parseInt(selectedCategory2);
  var request = {
    TrendyolCategoryId: y,
    CategoryId: x,
  }
  
  $.ajax({
    url: '/SendCategory/SendCt',
    type: 'POST',
    data: { request:request },
    success: function (result) {

      console.log("işlem başarılııııııı");
    },
    error: function (xhr, status, error) {
      console.log("işlem başarısızııııııı");
    }
  });
}

function sendBrand() {
  // Seçili değeri almak için:
  var selectedBrandId = document.getElementById("BrandId").value;
  var selectedBrandId2 = document.getElementById("TrendYolBrandId").value;
  var x = parseInt(selectedBrandId);
  var y = parseInt(selectedBrandId2);
  var request = {
    TrendyolBrandId: y,
    BrandId: x,
  }

  $.ajax({
    url: '/SendBrand/SendBr',
    type: 'POST',
    data: { request: request },
    success: function (result) {

      console.log("işlem başarılııııııı");
    },
    error: function (xhr, status, error) {
      console.log("işlem başarısızııııııı");
    }
  });
}










