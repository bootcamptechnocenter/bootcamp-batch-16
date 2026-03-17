(() => {
  const initSelect2 = (element, placeholder, requestBuilder) => {
    const $element = $(element);
    const ajaxUrl = $element.data("ajax-url");

    if (!ajaxUrl) {
      return;
    }

    $element.select2({
      placeholder: placeholder,
      allowClear: true,
      width: "100%",
      ajax: {
        url: ajaxUrl,
        dataType: "json",
        delay: 250,
        data: function (params) {
          const request = {
            search: params.term || "",
            page: params.page || 1,
            limit: 10,
          };

          if (requestBuilder) {
            requestBuilder(request);
          }

          return request;
        },
        processResults: function (data, params) {
          params.page = params.page || 1;

          return {
            results: data.items,
            pagination: {
              more: data.hasMore,
            },
          };
        },
        cache: true,
      },
    });

    const preserveCurrentValue = $element.find("option[selected]").length > 0;
    if (!preserveCurrentValue && (!$element.val() || $element.val() === "0")) {
      $element.val(null).trigger("change");
    }
  };

  $(document).ready(function () {
    const $brand = $("[data-select2-stock-brand='true']");
    const $type = $("[data-select2-stock-type='true']");
    const $model = $("[data-select2-stock-model='true']");

    if ($brand.length > 0) {
      initSelect2($brand, "-- Select Brand --");
    }

    if ($type.length > 0) {
      initSelect2($type, "-- Select Type --", (request) => {
        const brandId = $brand.val();
        if (brandId && brandId !== "0") {
          request.brandId = brandId;
        }
      });

      const hasBrandValue = $brand.val() && $brand.val() !== "0";
      $type.prop("disabled", !hasBrandValue);
    }

    if ($model.length > 0) {
      initSelect2($model, "-- Select Model --", (request) => {
        const typeId = $type.val();
        if (typeId && typeId !== "0") {
          request.typeId = typeId;
        }
      });

      const hasTypeValue = $type.val() && $type.val() !== "0";
      $model.prop("disabled", !hasTypeValue);
    }

    if ($brand.length > 0 && $type.length > 0) {
      $brand.on("change", function () {
        $type.val(null).trigger("change");
        $model.val(null).trigger("change");

        const hasBrand = $brand.val() && $brand.val() !== "0";
        $type.prop("disabled", !hasBrand);
        $model.prop("disabled", true);
      });
    }

    if ($type.length > 0 && $model.length > 0) {
      $type.on("change", function () {
        $model.val(null).trigger("change");

        const hasType = $type.val() && $type.val() !== "0";
        $model.prop("disabled", !hasType);
      });
    }
  });
})();
