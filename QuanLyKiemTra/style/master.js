function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const content = document.querySelector('.content');
    const menuToggle = document.querySelector('.menu-toggle');
    sidebar.classList.toggle('active');
    content.classList.toggle('sidebar-active');
    menuToggle.classList.toggle('hidden');
}

document.querySelectorAll('.dropdown-toggle').forEach(button => {
    button.addEventListener('click', function (e) {
        e.preventDefault();
        const submenu = this.nextElementSibling;
        const isActive = submenu.classList.contains('active');
        document.querySelectorAll('.submenus').forEach(sub => sub.classList.remove('active'));
        document.querySelectorAll('.dropdown-toggle').forEach(toggle => toggle.classList.remove('active'));
        if (!isActive) {
            submenu.classList.add('active');
            this.classList.add('active');
        }
    });
});

document.getElementById('notificationBtn').addEventListener('click', function (e) {
    e.preventDefault();
    const dropdown = document.getElementById('notificationDropdown');
    dropdown.classList.toggle('active');
});

document.addEventListener('click', function (e) {
    const notificationDropdown = document.getElementById('notificationDropdown');
    const notificationBtn = document.getElementById('notificationBtn');
    const sidebar = document.getElementById('sidebar');
    const menuToggle = document.querySelector('.menu-toggle');
    if (!notificationDropdown.contains(e.target) && e.target !== notificationBtn && !notificationBtn.contains(e.target)) {
        notificationDropdown.classList.remove('active');
    }
    if (!sidebar.contains(e.target) && !menuToggle.contains(e.target)) {
        document.querySelectorAll('.submenus').forEach(sub => sub.classList.remove('active'));
        document.querySelectorAll('.dropdown-toggle').forEach(toggle => toggle.classList.remove('active'));
        sidebar.classList.remove('active');
        document.querySelector('.content').classList.remove('sidebar-active');
        menuToggle.classList.remove('hidden');
    }
});

document.querySelectorAll('.submenus a').forEach(link => {
    link.addEventListener('click', () => {
        document.querySelectorAll('.submenus').forEach(sub => sub.classList.remove('active'));
        document.querySelectorAll('.dropdown-toggle').forEach(toggle => toggle.classList.remove('active'));
        document.getElementById('sidebar').classList.remove('active');
        document.querySelector('.content').classList.remove('sidebar-active');
        document.querySelector('.menu-toggle').classList.remove('hidden');
    });
});

const sidebarLinks = document.querySelectorAll('.sidebar a');
const currentPath = window.location.pathname.split('/').pop();
sidebarLinks.forEach(link => {
    const linkPath = link.getAttribute('href') || link.getAttribute('NavigateUrl');
    if (linkPath && linkPath.replace(/^~?\//, '') === currentPath) {
        link.classList.add('active');
        const parentSubmenu = link.closest('.submenus');
        if (parentSubmenu) {
            parentSubmenu.classList.add('active');
            parentSubmenu.previousElementSibling.classList.add('active');
        }
    }
});


// Khởi tạo Toastr và hiển thị thông báo từ HiddenField
function initializeToastr(hfMessageId, hfMessageTypeId) {
    // Cấu hình Toastr mặc định
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 5000,
        preventDuplicates: true
    };

    // Kiểm tra và hiển thị thông báo từ HiddenField
    var message = document.getElementById(hfMessageId)?.value;
    var messageType = document.getElementById(hfMessageTypeId)?.value;
    if (message) {
        if (messageType === 'error') {
            toastr.error(message, 'Lỗi');
        } else {
            toastr.success(message, 'Thành công');
        }
        // Xóa giá trị HiddenField sau khi hiển thị
        document.getElementById(hfMessageId).value = '';
        document.getElementById(hfMessageTypeId).value = '';
    }
}

// Hàm chung để hiển thị Toastr từ các trang con
function showToastr(message, type) {
    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: 'toast-top-right',
        timeOut: 5000,
        preventDuplicates: true
    };
    if (type === 'error') {
        toastr.error(message, 'Lỗi');
    } else {
        toastr.success(message, 'Thành công');
    }
}

// Khởi tạo tất cả khi trang tải
document.addEventListener('DOMContentLoaded', function () {
    toggleSidebar(); // Gắn sự kiện toggle sidebar
    initializeDropdownMenus();
    initializeClickOutside();
    initializeSubmenuLinks();
    highlightActiveLink();
});