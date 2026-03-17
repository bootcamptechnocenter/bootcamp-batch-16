(() => {
  const initBrandSelect2 = (element) => {
    const $element = $(element);
    const ajaxUrl = $element.data('ajax-url');

    if (!ajaxUrl) {
      return;
    }

    $element.select2({
      placeholder: '-- Select Brand --',
      allowClear: true,
      width: '100%',
      ajax: {
        url: ajaxUrl,
        dataType: 'json',
        delay: 250,
        data: function (params) {
          return {
            search: params.term || '',
            page: params.page || 1,
            limit: 10
          };
        },
        processResults: function (data, params) {
          params.page = params.page || 1;

          return {
            results: data.items,
            pagination: {
              more: data.hasMore
            }
          };
        },
        cache: true
      }
    });

    if (!$element.val() || $element.val() === '0') {
      $element.val(null).trigger('change');
    }
  };

  $(document).ready(function () {
    $('[data-select2-brand="true"]').each(function () {
      initBrandSelect2(this);
    });
  });
})();
