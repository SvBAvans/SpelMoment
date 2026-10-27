document.addEventListener("DOMContentLoaded", () => {
    const gamesContainer = document.getElementById("games-container");
    const addGameButton = document.getElementById("add-game");

    if (!gamesContainer || !addGameButton) return;

    const form = gamesContainer.closest("form");

    function updateGameIndexes() {
        const games = gamesContainer.querySelectorAll(".game-item");

        games.forEach((game, index) => {
            game.querySelector(".game-title").textContent = `Spel ${index + 1}`;

            game.querySelectorAll("[name]").forEach(field => {
                field.name = field.name.replace(
                    /Games\[\d+\]/g,
                    `Games[${index}]`
                );
            });

            game.querySelectorAll("[id]").forEach(field => {
                field.id = field.id.replace(
                    /Games_\d+__/g,
                    `Games_${index}__`
                );
            });

            game.querySelectorAll("label[for]").forEach(label => {
                label.htmlFor = label.htmlFor.replace(
                    /Games_\d+__/g,
                    `Games_${index}__`
                );
            });

            game.querySelectorAll("[data-valmsg-for]").forEach(span => {
                const value = span.getAttribute("data-valmsg-for");

                span.setAttribute(
                    "data-valmsg-for",
                    value.replace(/Games\[\d+\]/g, `Games[${index}]`)
                );
            });
        });
    }

    function refreshValidation() {
        if (
            window.jQuery &&
            $.validator &&
            $.validator.unobtrusive
        ) {
            const $form = $(form);

            $form.removeData("validator");
            $form.removeData("unobtrusiveValidation");

            $.validator.unobtrusive.parse($form);
        }
    }

    function clearGameFields(game) {
        game.querySelectorAll("input, textarea, select").forEach(field => {
            if (field.type === "checkbox") {
                field.checked = false;
            } else {
                field.value = "";
            }

            field.classList.remove("input-validation-error");
            field.removeAttribute("aria-invalid");
        });

        // Een nieuw spel mag geen bestaand Game-ID hebben.
        const idField = game.querySelector('input[name$=".Id"]');

        if (idField) {
            idField.value = "";
        }

        game.querySelectorAll(
            ".field-validation-error, .field-validation-valid"
        ).forEach(span => {
            span.textContent = "";
            span.classList.remove("field-validation-error");
            span.classList.add("field-validation-valid");
        });
    }

    addGameButton.addEventListener("click", () => {
        const firstGame = gamesContainer.querySelector(".game-item");

        if (!firstGame) return;

        const newGame = firstGame.cloneNode(true);

        clearGameFields(newGame);

        gamesContainer.appendChild(newGame);

        updateGameIndexes();
        refreshValidation();
    });

    gamesContainer.addEventListener("click", event => {
        const removeButton = event.target.closest(".remove-game");

        if (!removeButton) return;

        const games = gamesContainer.querySelectorAll(".game-item");

        if (games.length <= 1) {
            alert("Voeg minimaal één spel toe.");
            return;
        }

        const game = removeButton.closest(".game-item");

        if (game) {
            game.remove();
        }

        updateGameIndexes();
        refreshValidation();
    });

    if (form) {
        form.addEventListener("submit", () => {
            updateGameIndexes();
        });
    }

    updateGameIndexes();
});