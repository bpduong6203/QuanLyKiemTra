$(document).ready(function () {
    // Hàm chuẩn hóa tiếng Việt
    function normalizeVietnamese(str) {
        if (!str) return '';
        return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase().trim();
    }

    // Hàm khởi tạo tìm kiếm, sắp xếp và checkbox cho danh sách
    function initListSearch(config) {
        var $container = $(config.container);
        var $searchInput = $(config.searchInput);
        var $sortButton = $(config.sortButton);
        var $hiddenField = $(config.hiddenField);
        var itemSelector = config.itemSelector;
        var checkboxSelector = config.checkboxSelector;

        // Tìm kiếm
        $searchInput.on('input', function () {
            var searchText = normalizeVietnamese($(this).val());
            $container.find(itemSelector).each(function () {
                var itemName = normalizeVietnamese($(this).data('name'));
                var isMatch = searchText === '' || itemName.includes(searchText);
                $(this).toggle(isMatch);
            });
        });

        // Sắp xếp A-Z
        $sortButton.click(function () {
            var items = $container.find(itemSelector).get();
            items.sort(function (a, b) {
                var nameA = normalizeVietnamese($(a).data('name'));
                var nameB = normalizeVietnamese($(b).data('name'));
                return nameA < nameB ? -1 : nameA > nameB ? 1 : 0;
            });
            $container.find(itemSelector).remove();
            $.each(items, function (i, item) {
                $container.append(item);
            });
        });

        // Đảm bảo chỉ một checkbox được chọn
        $container.on('change', checkboxSelector, function () {
            var selectedIds = [];
            $container.find(checkboxSelector + ':checked').each(function () {
                selectedIds.push($(this).data('id'));
            });
            $hiddenField.val(selectedIds.join(','));
        });
    }

    // Khởi tạo cho danh sách đơn vị (pageKeHoach)
    initListSearch({
        container: '.form-groups.lg:has(.unit-item)',
        searchInput: '#txtSearchUnit',
        sortButton: '#btnSortUnit',
        hiddenField: '#hfSelectedDonVi',
        itemSelector: '.unit-item',
        checkboxSelector: '.unit-checkbox'
    });

    // Khởi tạo cho danh sách kế hoạch (pagePhanCong)
    initListSearch({
        container: '.form-groups.lg:has(.plan-item)',
        searchInput: '#txtSearchPlan',
        sortButton: '#btnSortPlan',
        hiddenField: '#hfSelectedKeHoach',
        itemSelector: '.plan-item',
        checkboxSelector: '.plan-checkbox'
    });

    // Khởi tạo cho danh sách câu hỏi (pageBoCauHoi)
    initListSearch({
        container: '.question-list',
        searchInput: '#txtSearchQuestion',
        sortButton: '#btnSortQuestion',
        hiddenField: '#hfSelectedQuestions',
        itemSelector: '.question-item',
        checkboxSelector: '.question-checkbox'
    });
});