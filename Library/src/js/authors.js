// Получение всех авторов
async function getAuthors() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/authors", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const authors = await response.json(); // получаем массив книг
        const rows = document.querySelector("#authors"); // выбираем таблицу с селектором books
        authors.forEach(author => rows.append(appendAuthorRow(author))); // вставляем книги в таблицу в виде строк
    }
}

// Получение одного автора
async function getAuthor(id) {
    // отправляем запрос
    const response = await fetch(`/api/authors/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const author = await response.json();
        // записываем данные в форму
        document.getElementById("authorId").value = author.id;
        document.getElementById("authorName").value = author.name;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}

// Добавление автора
async function createAuthor(name) {

    const response = await fetch("/api/authors", {
        // отправляем полученные данные (параметры) в бэк
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: name
        })
    });
    if (response.ok === true) {
        // сохраняет ответ из бэка в объект book
        const author = await response.json();
        // присоединяем новую строку
        document.querySelector("#authors").append(appendAuthorRow(author));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Удаление автора
async function deleteAuthor(id) {
    // получаем данные
    const response = await fetch(`/api/authors/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const author = await response.json(); // сохраняем данные из запроса
        document.querySelector(`tr[data-rowid='${author.id}']`).remove(); // удаляем элемент из представления
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Изменение автора
async function editAuthor(id, name) {
    // сохраняем новые данные (обновляем элемент)
    const response = await fetch("/api/authors", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            Id: id,
            name: name
        })
    });
    if (response.ok === true) {
        // получаем измененный объект
        const author = await response.json();
        // заменяем строку с объектом на новую
        document.querySelector(`tr[data-rowid='${author.id}']`).replaceWith(appendAuthorRow(author));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}


function appendAuthorRow(author) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", author.id);

    const authorTd = document.createElement("td");
    authorTd.append(author.name);
    tr.append(authorTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("button");
    editLink.append("Edit");
    editLink.addEventListener("click", async() => await getAuthor(author.id));
    editLink.classList.add("button");
    editLink.classList.add("library-items__edit-link");
    linksTd.append(editLink);

    const removeLink = document.createElement("button");
    removeLink.append("Delete");
    removeLink.addEventListener("click", async () => await deleteAuthor(author.id));
    removeLink.classList.add("button");
    removeLink.classList.add("library-items__remove-link");
    linksTd.append(removeLink);

    tr.appendChild(linksTd);

    return tr;
}

function reset() {
    document.getElementById("authorId").value =
        document.getElementById("authorName").value = "";
}



// сброс значений формы Author - вешаем на кнопку 'Reset
document.getElementById("resetAuthorBtn").addEventListener("click", () =>  reset());

// отправка формы Author  - вешаем функцию на кнопку "Save"
document.getElementById("saveAuthorBtn").addEventListener("click", async () => {
    // сохраняем данные из формы в переменную
    const id = document.getElementById("authorId").value;
    const name = document.getElementById("authorName").value;

    if (id === "")
        // если id не проставлено - то есть мы просто заполняем форму - создаем нового автора
        await createAuthor(name);
    else
        // если id стоит -  то есть мы получили данные при помощи кнопки "изменить" - обновляем автора
        await editAuthor(id, name);
    reset();
});

getAuthors();
