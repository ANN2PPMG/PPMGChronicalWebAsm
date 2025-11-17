export function initializeHistoryTimeline() {
    const list = document.getElementById('timeline-list');
    const previewImg = document.getElementById('preview-image');
    const previewCaption = document.getElementById('preview-caption');
    
    if (!list || !previewImg || !previewCaption) {
        return;
    }

    const defaultSrc = previewImg.getAttribute('src');
    const defaultCaption = previewCaption.textContent;

    function showPreview(imgSrc, captionText) {
        if (imgSrc) {
            previewImg.style.opacity = '0';
            setTimeout(() => {
                previewImg.src = imgSrc;
                previewCaption.textContent = captionText || '';
                previewImg.style.opacity = '1';
            }, 100);
        } else {
            hidePreview();
        }
    }

    function hidePreview() {
        previewImg.style.opacity = '0';
        setTimeout(() => {
            previewImg.src = defaultSrc;
            previewCaption.textContent = defaultCaption;
            previewImg.style.opacity = '1';
        }, 100);
    }

    list.querySelectorAll('li').forEach(li => {
        const img = li.dataset.img;
        const caption = li.dataset.caption || '';

        li.addEventListener('mouseenter', () => showPreview(img, caption));
        li.addEventListener('focus', () => showPreview(img, caption));
        li.addEventListener('mouseleave', () => hidePreview());
        li.addEventListener('blur', () => hidePreview());

        li.addEventListener('click', (e) => {
            if (previewImg.src.endsWith(img)) {
                hidePreview();
            } else {
                showPreview(img, caption);
            }
        });
    });

    document.addEventListener('click', (e) => {
        if (!list.contains(e.target)) {
            hidePreview();
        }
    });
}