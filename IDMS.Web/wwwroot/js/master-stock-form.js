(() => {
    const brandSelect = document.getElementById('brandSelect');
    const typeSelect  = document.getElementById('typeSelect');
    const modelSelect = document.getElementById('modelSelect');

    if (!brandSelect || !typeSelect || !modelSelect) return;

    const brandAjaxUrl = brandSelect.dataset.ajaxUrl;
    const typeAjaxUrl  = typeSelect.dataset.ajaxUrl;
    const modelAjaxUrl = modelSelect.dataset.ajaxUrl;

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
            processResults: (data) => ({
                results:    data.items,
                pagination: { more: data.hasMore },
            }),
            cache: true,
        },
    });

    // ── Select2: Type (filtered by Brand) ─────────────────────────────────────

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
            processResults: (data) => ({
                results:    data.items,
                pagination: { more: data.hasMore },
            }),
            cache: false,
        },
    });

    // ── Select2: Model (filtered by Type) ─────────────────────────────────────

    $(modelSelect).select2({
        placeholder: '-- Select Model --',
        allowClear: true,
        ajax: {
            url: modelAjaxUrl,
            dataType: 'json',
            delay: 300,
            data: params => ({
                search: params.term ?? '',
                page:   params.page  ?? 1,
                limit:  10,
                typeId: $(typeSelect).val() ?? 0,
            }),
            processResults: (data) => ({
                results:    data.items,
                pagination: { more: data.hasMore },
            }),
            cache: false,
        },
    });

    // ── Cascade: Brand → reset Type & Model ───────────────────────────────────

    $(brandSelect).on('change', () => {
        $(typeSelect).val(null).trigger('change');
        $(modelSelect).val(null).trigger('change');
    });

    // ── Cascade: Type → reset Model ───────────────────────────────────────────

    $(typeSelect).on('change', () => {
        $(modelSelect).val(null).trigger('change');
    });

})();