// Bootstrap Icons helper for admin UI.
// Extracts icon class names from the loaded Bootstrap Icons stylesheet.
// Falls back gracefully if fetch/CORS fails.

(function () {
  function extractIconNamesFromCss(cssText) {
    // Matches: .bi-alarm::before { ... }
    const regex = /\.bi-([a-z0-9-]+)::before\s*\{/gi;
    const names = new Set();
    let match;
    while ((match = regex.exec(cssText)) !== null) {
      if (match[1]) names.add(match[1]);
    }
    return Array.from(names).sort();
  }

  async function fetchBootstrapIconsCss() {
    // Keep in sync with App.razor link.
    const url = "https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css";
    const res = await fetch(url, { cache: "force-cache" });
    if (!res.ok) throw new Error(`Failed to fetch bootstrap-icons.css: ${res.status}`);
    return await res.text();
  }

  window.masBootstrapIcons = {
    // Returns array of full class strings: ["bi bi-alarm", ...]
    getIconClasses: async function () {
      try {
        const cssText = await fetchBootstrapIconsCss();
        const names = extractIconNamesFromCss(cssText);
        return names.map(n => `bi bi-${n}`);
      } catch {
        // Minimal fallback list (common icons) to keep UI functional.
        return [
          "bi bi-tags",
          "bi bi-grid",
          "bi bi-house",
          "bi bi-bag",
          "bi bi-box-seam",
          "bi bi-briefcase",
          "bi bi-chat-dots",
          "bi bi-envelope",
          "bi bi-gear",
          "bi bi-image",
          "bi bi-lightning",
          "bi bi-palette",
          "bi bi-pen",
          "bi bi-rocket",
          "bi bi-star",
          "bi bi-telephone",
          "bi bi-tools"
        ];
      }
    }
  };
})();
