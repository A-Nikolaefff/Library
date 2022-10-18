// Получение всех жанров
async function getGenres() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/genres", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const genres = await response.json(); // получаем массив жанров
        const rows = document.querySelector("#genres"); // выбираем таблицу с селектором genres
        genres.forEach(genre => rows.append(appendGenreRow(genre))); // вставляем жанры в таблицу в виде строк
    }
}

// Получение одного автора
async function getGenre(id) {
    // отправляем запрос
    const response = await fetch(`/api/genres/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const genre = await response.json();
        // записываем данные в форму
        document.getElementById("genreId").value = genre.id;
        document.getElementById("genreName").value = genre.name;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}

// Добавление автора
async function createGenre(name) {

    const response = await fetch("/api/genres", {
        // отправляем полученные данные (параметры) в бэк
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: name
        })
    });
    if (response.ok === true) {
        // сохраняет ответ из бэка в объект book
        const genre = await response.json();
        // присоединяем новую строку
        document.querySelector("#genres").append(appendGenreRow(genre));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Удаление автора
async function deleteGenre(id) {
    // получаем данные
    const response = await fetch(`/api/genres/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const genre = await response.json(); // сохраняем данные из запроса
        document.querySelector(`tr[data-rowid='${genre.id}']`).remove(); // удаляем элемент из представления
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Изменение автора
async function editGenre(id, name) {
    // сохраняем новые данные (обновляем элемент)
    const response = await fetch("/api/genres", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            Id: id,
            name: name
        })
    });
    if (response.ok === true) {
        // получаем измененный объект
        const genre = await response.json();
        // заменяем строку с объектом на новую
        document.querySelector(`tr[data-rowid='${genre.id}']`).replaceWith(appendGenreRow(genre));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}


function appendGenreRow(genre) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", genre.id);

    const genreTd = document.createElement("td");
    genreTd.append(genre.name);
    tr.append(genreTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("button");
    editLink.append("Edit");
    editLink.addEventListener("click", async() => await getGenre(genre.id));
    editLink.classList.add("button");
    editLink.classList.add("library-items__edit-link");
    linksTd.append(editLink);

    const removeLink = document.createElement("button");
    removeLink.append("Delete");
    removeLink.addEventListener("click", async () => await deleteGenre(genre.id));
    removeLink.classList.add("button");
    removeLink.classList.add("library-items__remove-link");
    linksTd.append(removeLink);

    tr.appendChild(linksTd);

    return tr;
}

function reset() {
    document.getElementById("genreId").value =
        document.getElementById("genreName").value = "";
}



// сброс значений формы genre - вешаем на кнопку 'Reset
document.getElementById("resetGenreBtn").addEventListener("click", () =>  reset());

// отправка формы genre  - вешаем функцию на кнопку "Save"
document.getElementById("saveGenreBtn").addEventListener("click", async () => {
    // сохраняем данные из формы в переменную
    const id = document.getElementById("genreId").value;
    const name = document.getElementById("genreName").value;

    if (id === "")
        // если id не проставлено - то есть мы просто заполняем форму - создаем нового автора
        await createGenre(name);
    else
        // если id стоит -  то есть мы получили данные при помощи кнопки "изменить" - обновляем автора
        await editGenre(id, name);
    reset();
});

getGenres();
