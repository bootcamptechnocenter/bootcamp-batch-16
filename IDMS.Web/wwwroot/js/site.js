// IDMS — jQuery AJAX pagination & UX enhancements

// ── Toast utility ──────────────────────────────────────────────────────
var _toastTimer = {};
var _toastSeq = 0;

function idmsToast(message, type, title) {
  type = type || "info";
  var icons = {
    success: "bi-check-circle-fill",
    danger: "bi-x-circle-fill",
    warning: "bi-exclamation-triangle-fill",
    info: "bi-info-circle-fill",
  };
  var titles = {
    success: "Success",
    danger: "Error",
    warning: "Warning",
    info: "Info",
  };
  var id = "idms-t-" + ++_toastSeq;
  var $t = $(
    '<div class="idms-toast toast-' +
      type +
      '" id="' +
      id +
      '">' +
      '<i class="bi ' +
      (icons[type] || icons.info) +
      ' toast-icon"></i>' +
      '<div class="toast-body">' +
      '<div class="toast-title">' +
      (title || titles[type] || "Info") +
      "</div>" +
      '<div class="toast-msg">' +
      message +
      "</div>" +
      "</div>" +
      '<button class="toast-close" onclick="idmsHideToast(this)"><i class="bi bi-x"></i></button>' +
      "</div>",
  );
  $("#toast-container").append($t);
  // Trigger reflow so transition fires
  $t[0].offsetHeight;
  $t.addClass("show");
  _toastTimer[id] = setTimeout(function () {
    idmsHideToast($t.find(".toast-close")[0]);
  }, 4000);
}

function idmsHideToast(btn) {
  var $t = $(btn).closest(".idms-toast");
  var id = $t.attr("id");
  clearTimeout(_toastTimer[id]);
  $t.removeClass("show");
  setTimeout(function () {
    $t.remove();
  }, 300);
}

$(function () {
  // ── Show server-side TempData toasts on page load ────────────────────
  $("#toast-container .idms-toast[data-autohide]").each(function () {
    var $t = $(this);
    var id = "idms-t-" + ++_toastSeq;
    $t.attr("id", id);
    $t[0].offsetHeight;
    $t.addClass("show");
    _toastTimer[id] = setTimeout(function () {
      idmsHideToast($t.find(".toast-close")[0]);
    }, 4000);
  });

  // ── AJAX pagination & limit ──────────────────────────────────────────
  function loadTablePage(url) {
    var $wrapper = $("#table-wrapper");
    if (!$wrapper.length) return;

    $("#ajax-loader").addClass("show");

    $.ajax({
      url: url,
      headers: { "X-Requested-With": "XMLHttpRequest" },
      success: function (html) {
        $wrapper.html(html);
        bindTableEvents();
      },
      error: function () {
        window.location.href = url;
      },
      complete: function () {
        $("#ajax-loader").removeClass("show");
      },
    });
  }

  function bindTableEvents() {
    var $wrapper = $("#table-wrapper");

    $wrapper
      .off("click.idms", "a.page-link")
      .on("click.idms", "a.page-link", function (e) {
        e.preventDefault();
        if ($(this).closest(".page-item").hasClass("disabled")) return;
        loadTablePage($(this).attr("href"));
      });

    $wrapper
      .off("change.idms", 'select[name="limit"]')
      .on("change.idms", 'select[name="limit"]', function () {
        var $form = $(this).closest("form");
        var url = $form.attr("action") + "?" + $form.serialize();
        loadTablePage(url);
      });
  }

  bindTableEvents();

  // ── Auto-dismiss legacy alerts ────────────────────────────────────────
  setTimeout(function () {
    $(".alert-dismissible").fadeOut(400, function () {
      $(this).remove();
    });
  }, 4000);
});
