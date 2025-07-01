<script>
        // Navigation between pages
        document.querySelectorAll('.sidebar-menu a').forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();

            // Update active state in sidebar
            document.querySelectorAll('.sidebar-menu a').forEach(item => {
                item.classList.remove('active');
            });
            this.classList.add('active');

            // Update header title
            const headerTitle = document.querySelector('.header-title');
            headerTitle.textContent = this.textContent.trim();

            // Hide all pages
            document.querySelectorAll('.page-content').forEach(page => {
                page.style.display = 'none';
            });

            // Show selected page
            const targetPage = document.querySelector(this.getAttribute('href'));
            if (targetPage) {
                targetPage.style.display = 'block';
            }
        });
        });

        // View student analytics
        document.querySelectorAll('.view-student').forEach(button => {
        button.addEventListener('click', function () {
            // Hide all pages
            document.querySelectorAll('.page-content').forEach(page => {
                page.style.display = 'none';
            });

            // Show analytics page
            const analyticsPage = document.querySelector('#student-analytics');
            analyticsPage.style.display = 'block';

            // Update header
            const headerTitle = document.querySelector('.header-title');
            headerTitle.textContent = 'Student Analytics';

            // Update student details
            const studentId = this.getAttribute('data-student-id');
            const studentName = this.getAttribute('data-student-name');

            document.querySelector('#student-id').textContent = studentId;
            document.querySelector('#student-name').textContent = studentName;
        });
        });

    // Back to students page
    document.querySelector('.back-to-students').addEventListener('click', function() {
        // Hide all pages
        document.querySelectorAll('.page-content').forEach(page => {
            page.style.display = 'none';
        });

    // Show students page
    const studentsPage = document.querySelector('#students');
    studentsPage.style.display = 'block';

    // Update header
    const headerTitle = document.querySelector('.header-title');
    headerTitle.textContent = 'Students';

            // Update sidebar active state
            document.querySelectorAll('.sidebar-menu a').forEach(item => {
        item.classList.remove('active');
    if(item.getAttribute('href') === '#students') {
        item.classList.add('active');
                }
            });
        });
        // All your JavaScript from the original script tag
document.querySelectorAll('.model-tab').forEach(tab => {
    tab.addEventListener('click', () => {
        document.querySelectorAll('.model-tab').forEach(t => t.classList.remove('active'));
        document.querySelectorAll('.model-content').forEach(c => c.classList.remove('active'));
        
        tab.classList.add('active');
        const tabId = tab.getAttribute('data-tab');
        document.getElementById(`${tabId}-content`).classList.add('active');
    });
});

// Smooth scrolling
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
        e.preventDefault();
        const targetId = this.getAttribute('href');
        if(targetId === '#') return;
        const targetElement = document.querySelector(targetId);
        if(targetElement) {
            window.scrollTo({
                top: targetElement.offsetTop,
                behavior: 'smooth'
            });
        }
    });
});
</script>