document.addEventListener('DOMContentLoaded', () => {
  const sidebar = document.querySelector('.sidebar');
  const menuToggle = document.querySelector('[data-menu-toggle]');

  if (sidebar) {
    const currentPage = window.location.pathname.split('/').pop() || 'index.html';
    sidebar.querySelectorAll('.side-nav a').forEach((link) => {
      const linkPage = new URL(link.href, window.location.href).pathname.split('/').pop();
      const active = linkPage === currentPage;
      link.classList.toggle('active', active);
      if (active) link.setAttribute('aria-current', 'page');
      else link.removeAttribute('aria-current');
    });
  }

  if (menuToggle && sidebar) {
    menuToggle.setAttribute('aria-expanded', 'false');
    menuToggle.addEventListener('click', () => {
      const open = sidebar.classList.toggle('open');
      menuToggle.setAttribute('aria-expanded', String(open));
    });

    const closeMenu = () => {
      sidebar.classList.remove('open');
      menuToggle.setAttribute('aria-expanded', 'false');
    };
    document.addEventListener('keydown', (event) => {
      if (event.key === 'Escape') closeMenu();
    });
    sidebar.querySelectorAll('a').forEach((link) => link.addEventListener('click', closeMenu));
  }
});
