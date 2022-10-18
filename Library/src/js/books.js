// Получение всех книг
async function getBooks() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/books", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const books = await response.json(); // получаем массив книг
        const rows = document.querySelector("#books"); // выбираем таблицу с селектором books
        books.forEach(book => rows.append(appendBookRow(book))); // вставляем книги в таблицу в виде строк
    }
}

// Получение одной книги
async function getBook(id) {
    // отправляем запрос
    const response = await fetch(`/api/Books/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const book = await response.json();
        // записываем данные в форму
        document.getElementById("bookId").value = book.id;
        document.getElementById("bookTitle").value = book.title;
        document.getElementById("authorSelect").value = book.authorId;
        document.getElementById("genreSelect").value = book.genreId;
        document.getElementById("bookAmount").value = book.amount;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}

// Добавление книги
async function createBook(title, authorId, genreId, amount) {

    const response = await fetch("/api/books", {
        // отправляем полученные данные (параметры) в бэк
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            title: title,
            authorId: authorId,
            genreId: genreId,
            amount: amount
        })
    });
    if (response.ok === true) {
        // сохраняет ответ из бэка в объект book
        const book = await response.json();
        // присоединяем новую строку
        document.querySelector("#books").append(appendBookRow(book));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Удаление книги
async function deleteBook(id) {
    // получаем данные
    const response = await fetch(`/api/books/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const book = await response.json(); // сохраняем данные из запроса
        document.querySelector(`tr[data-rowid='${book.id}']`).remove(); // удаляем элемент из представления
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

// Изменение книги
async function editBook(id, title, authorId, genreId, amount) {
    // сохраняем новые данные (обновляем элемент)
    const response = await fetch("/api/books", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            Id: id,
            title: title,
            authorId: authorId,
            genreId: genreId,
            amount: amount
        })
    });
    if (response.ok === true) {
        // получаем измененный объект
        const book = await response.json();
        // заменяем строку с объектом на новую
        document.querySelector(`tr[data-rowid='${book.id}']`).replaceWith(appendBookRow(book));
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message);
    }
}

function appendBookRow(book) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", book.id);

    const titleTd = document.createElement("td");
    titleTd.append(book.title);
    tr.append(titleTd);

    const author = document.createElement("td");
    author.append(book.author.name);
    tr.append(author);

    const genre = document.createElement("td");
    genre.append(book.genre.name);
    tr.append(genre);

    const amount = document.createElement("td");
    amount.append(book.amount);
    tr.append(amount);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("button");
    editLink.append("Edit");
    editLink.addEventListener("click", async() => await getBook(book.id));
    editLink.classList.add("button");
    editLink.classList.add("library-items__edit-link");
    linksTd.append(editLink);

    const removeLink = document.createElement("button");
    removeLink.append("Delete");
    removeLink.addEventListener("click", async () => await deleteBook(book.id));
    removeLink.classList.add("button");
    removeLink.classList.add("library-items__remove-link");
    linksTd.append(removeLink);

    tr.appendChild(linksTd);

    return tr;
}

// сброс данных формы после отправки
function reset() {
    document.getElementById("bookId").value =
        document.getElementById("bookTitle").value =
            document.getElementById("authorSelect").value =
                document.getElementById("genreSelect").value =
                    document.getElementById("bookAmount").value = "";
}

async function getAuthorOptions(){
    const response = await fetch("/api/authors", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const authors = await response.json(); // получаем массив авторов
        var authorSelect = document.querySelector("#authorSelect"); // выбираем таблицу с селектором authorSelect
        authors.forEach(author =>  authorSelect.options[authorSelect.options.length]
            = new Option(author.name, author.id)); // вставляем авторов в селект
    }
}

async function getGenreOptions(){
    const response = await fetch("/api/genres", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        // получаем данные
        const genres = await response.json(); // получаем массив жанров
        var genreSelect = document.querySelector("#genreSelect"); // выбираем таблицу с селектором genreSelect
        genres.forEach(genre => genreSelect.options[genreSelect.options.length]
            = new Option(genre.name, genre.id)); // вставляем жанры в селект
    }
}

// сброс значений формы Book - вешаем на кнопку 'Reset
document.getElementById("resetBookBtn").addEventListener("click", () =>  reset());

// отправка формы Book - вешаем функцию на кнопку "Save"
document.getElementById("saveBookBtn").addEventListener("click", async () => {
    // сохраняем данные из формы в переменную
    const id = document.getElementById("bookId").value;
    const title = document.getElementById("bookTitle").value;
    const authorId = document.getElementById("authorSelect").value;
    const genreId = document.getElementById("genreSelect").value;
    const amount = document.getElementById("bookAmount").value;

    if (id === "")
        // если id не проставлено - то есть мы просто заполняем форму - создаем новую книгу
        await createBook(title, authorId, genreId, amount);
    else
        // если id стоит -  то есть мы получили данные при помощи кнопки "изменить" - обновляем книгу
        await editBook(id, title, authorId, genreId, amount);
    reset();
});

getBooks();
getAuthorOptions();
getGenreOptions();
