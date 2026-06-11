(function () {
    document.addEventListener('click', function (event) {
        var button = event.target.closest('.integration-results-refresh');
        if (!button) {
            return;
        }

        event.preventDefault();
        refreshIntegrationResults(button);
    });

    function refreshIntegrationResults(button) {
        var panel = document.getElementById('integration-results-panel');
        if (!panel) {
            return;
        }

        var url = new URL(window.location.href);
        url.searchParams.set('handler', 'Results');

        var originalText = button.textContent;
        button.disabled = true;
        button.textContent = 'Обновление…';

        fetch(url.toString(), {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('HTTP ' + response.status);
                }

                return response.text();
            })
            .then(function (html) {
                var parser = new DOMParser();
                var documentFragment = parser.parseFromString(html, 'text/html');
                var newPanel = documentFragment.getElementById('integration-results-panel');

                if (!newPanel) {
                    throw new Error('Results panel not found in response.');
                }

                panel.replaceWith(newPanel);
            })
            .catch(function () {
                window.alert('Не удалось обновить результаты. Попробуйте ещё раз.');
            })
            .finally(function () {
                var activeButton = document.querySelector('.integration-results-refresh');
                if (activeButton) {
                    activeButton.disabled = false;
                    activeButton.textContent = originalText;
                }
            });
    }
})();
