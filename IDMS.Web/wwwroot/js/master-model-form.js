(() => {
  const initBrandSelect2 = (element) => {
    const $element = $(element);
    const ajaxUrl = $element.data("ajax-url");

    if (!ajaxUrl) {
      return;
    }

    $element.select2({
      placeholder: "-- Select Brand --",
      allowClear: true,
      width: "100%",
      ajax: {
        url: ajaxUrl,
        dataType: "json",
        delay: 250,
        data: function (params) {
          return {
            search: params.term || "",
            page: params.page || 1,
            limit: 10,
          };
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

  const initTypeSelect2 = (element, placeholderText) => {
    const $element = $(element);
    const ajaxUrl = $element.data("ajax-url");
    const hasBrandDependency = $element.data("select2-type-by-brand") === true;
    const brandSelector = $element.data("brand-selector");
    const $brandElement =
      hasBrandDependency && brandSelector ? $(brandSelector) : null;

    const getBrandId = () => {
      if (!$brandElement || $brandElement.length === 0) {
        return null;
      }

      const brandId = $brandElement.val();
      if (!brandId || brandId === "0") {
        return null;
      }

      return brandId;
    };

    const setTypeEnabledState = () => {
      if (!hasBrandDependency) {
        return;
      }

      const hasBrand = !!getBrandId();
      $element.prop("disabled", !hasBrand);
    };

    if (!ajaxUrl) {
      return;
    }

    $element.select2({
      placeholder: placeholderText,
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

          const brandId = getBrandId();
          if (brandId) {
            request.brandId = brandId;
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

    setTypeEnabledState();

    if (hasBrandDependency && $brandElement && $brandElement.length > 0) {
      $brandElement.on("change", function () {
        $element.val(null).trigger("change");
        setTypeEnabledState();
      });
    }
  };

  $(document).ready(function () {
    $('[data-select2-brand-model="true"]').each(function () {
      initBrandSelect2(this);
    });

    $('[data-select2-type="true"]').each(function () {
      initTypeSelect2(this, "-- Select Type --");
    });

    $('[data-select2-type-filter="true"]').each(function () {
      initTypeSelect2(this, "All Types");
    });
  });
})();
