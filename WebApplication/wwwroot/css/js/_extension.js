$(window).on("load", function () {
  // Custom Material Form
  $(".material-form .form-select").each(function () {
    let t = $(this);
    if (t[0].value != 0) {
      t.addClass("has-value");
    } else {
      t.removeClass("has-value");
    }
    t.on("change", function () {
      if (t[0].value != 0) {
        t.addClass("has-value");
      } else {
        t.removeClass("has-value");
      }
    });
  });
  $(".account-link").each(function () {
    let e = $(this);
    e.on("click", function () {
      console.log(e);
      e.parent().find(".dropdown-menu").slideToggle();
    });
  });
  // Img Cover
  $(".currentImg-link").each(function () {
    let e = $(this);
    e.on("click", function (i) {
      i.preventDefault();
      e.parents(".list-currentImg")
        .find(".currentImg-link")
        .not(e)
        .removeClass("active");
      e.toggleClass("active");
      if (e.hasClass("no-bg")) {
        e.removeClass("active");
      }
    });
  });
  // Social Stick
  $(".ball-show").each(function () {
    let t = $(this);
    t.on("click", function () {
      t.toggleClass("active"),
        t.parent().find(".pane-toggle").toggleClass("active");
    });
  });
  $(".faq-links").each(function () {
    let t = $(this);
    let collapse = t.data("toggle");
    t.on("click", function (e) {
      t.parents(".block-faq")
        .find(".faq-links")
        .not(t)
        .removeClass("open-toggle");
      t.toggleClass("open-toggle");
      t.parents(".block-faq").find(".faq-collapse").not(collapse).slideUp();
      t.parent().find(collapse).slideToggle();
      e.preventDefault();
    });
  });
  $(".content-ellips").each(function () {
    let e = $(this);
    let parent = e.parent();
    if (e.height() >= 1500) {
      e.addClass("textover");
      parent.find(".btn-toggle-content").removeClass("hidden");
    } else {
      e.removeClass("textover");
      parent.find(".btn-toggle-content").addClass("hidden");
    }
  });
  $(".nav-link").each(function () {
    let e = $(this);
    e.on("click", function () {
      let data = e.attr("data-bs-target");
      let parent = $(data).parent();
      if ($(data).height() >= 1500) {
        $(data).find(".content-ellips").addClass("textover");
        parent.find(".btn-toggle-content").removeClass("hidden");
      } else {
        $(data).removeClass("textover");
        parent.find(".btn-toggle-content").addClass("hidden");
      }
    });
  });
});
