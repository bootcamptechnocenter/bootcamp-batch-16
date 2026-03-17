(() => {
    const brandSelect = document.getElementById('brandSelect');
    const typeSelect  = document.getElementById('typeSelect');

    if (!brandSelect || !typeSelect) return;

    const brandAjaxUrl = brandSelect.dataset.ajaxUrl;
    const typeAjaxUrl  = typeSelect.dataset.ajaxUrl;

    // ── Select2: Brand ────────────────────────────────────────────────────────

    $(brandSelect).select2({
        placeholder: '-- Select Brand --',
        allowClear: true,
        ajax: {
            url: brandAjaxUrl,
            dataType: 'json',
            delay: 300,
            data: params => ({
                search: params.term ?? '',
                page:   params.page  ?? 1,
                limit:  10,
            }),
            processResults: (data, params) => ({
                results:    data.items,
                pagination: { more: data.hasMore },
            }),
            cache: true,
        },
    });

    // ── Select2: Type (filtered by selected Brand) ────────────────────────────

    $(typeSelect).select2({
        placeholder: '-- Select Type --',
        allowClear: true,
        ajax: {
            url: typeAjaxUrl,
            dataType: 'json',
            delay: 300,
            data: params => ({
                search:  params.term ?? '',
                page:    params.page  ?? 1,
                limit:   10,
                brandId: $(brandSelect).val() ?? 0,
            }),
            processResults: (data, params) => ({
                results:    data.items,
                pagination: { more: data.hasMore },
            }),
            cache: false,
        },
    });

    // ── Cascade: reset Type when Brand changes ────────────────────────────────

    $(brandSelect).on('change', () => {
        $(typeSelect).val(null).trigger('change');
    });

})();