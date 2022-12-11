window.onload = function () {
  // let prefixAside = document.getElementById("prefix_aside");
  // document.getElementById("bookmng").addEventListener("click", () => {
  //   window.location.href = `${prefixAside}/back-end/book/back_book_view.jsp`;
  // });
};

$(".label")
  .find("span")
  .click(function () {
    $(this).toggleClass("rotate");
  });
