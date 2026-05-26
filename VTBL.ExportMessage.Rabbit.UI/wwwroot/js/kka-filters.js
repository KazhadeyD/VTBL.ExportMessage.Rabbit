(function () {
    var input = document.getElementById('filter-id');
    if (!input) {
        return;
    }

    var maxHexLength = 32;

    function normalizeHex(value) {
        return (value || '').replace(/[^a-fA-F0-9]/g, '').slice(0, maxHexLength).toUpperCase();
    }

    function formatGuid(hex) {
        var parts = [
            hex.slice(0, 8),
            hex.slice(8, 12),
            hex.slice(12, 16),
            hex.slice(16, 20),
            hex.slice(20, 32)
        ];

        var result = parts[0];
        if (hex.length > 8) {
            result += '-' + parts[1];
        }
        if (hex.length > 12) {
            result += '-' + parts[2];
        }
        if (hex.length > 16) {
            result += '-' + parts[3];
        }
        if (hex.length > 20) {
            result += '-' + parts[4];
        }

        return result;
    }

    function applyMask() {
        var hex = normalizeHex(input.value);
        input.value = formatGuid(hex);
    }

    input.addEventListener('input', applyMask);
    input.addEventListener('paste', function (e) {
        e.preventDefault();
        var text = (e.clipboardData || window.clipboardData).getData('text');
        input.value = formatGuid(normalizeHex(text));
    });

    applyMask();
})();
