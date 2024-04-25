import axios, { AxiosResponse } from 'axios';

const baseURL = 'http://localhost:33755/api/book'

const headers: Readonly<Record<string, string | boolean>> = {
    Accept: "application/json",
    "Content-Type": "application/json; charset=utf-8",
    "Access-Control-Allow-Credentials": true,
    "X-Requested-With": "XMLHttpRequest"
  };

// Define la interfaz para los datos del libro
interface Book {
    // Define las propiedades de acuerdo a tu modelo de datos
    BookId: string;
    Title: string;
    FileDirection?: string;
    PublicationDate?: string;
    BookAuthorGuid?: string;
    AuthorName?: string;
    AuthorLastName?: string;
    AuthorBirthdate?: string;
}

// Define la interfaz para el comando de creación de libros
interface CreateBookCommand {
    // Define las propiedades de acuerdo a tu modelo de datos
    // Por ejemplo:
    Title: string;
    FileDirection?: string;
    // Agrega más propiedades según sea necesario
}

// Define la interfaz para el comando de actualización de libros
interface UpdateBookCommand {
    // Define las propiedades de acuerdo a tu modelo de datos
    // Por ejemplo:
    BookId: string;
    Title?: string;
    FileDirection?: string;
    // Agrega más propiedades según sea necesario
}

// Define la interfaz para el servicio de libros
interface BookService {
    insertBook(createBookCommand: CreateBookCommand): Promise<void>;
    updateBook(bookId: string, updateBookCommand: UpdateBookCommand): Promise<void>;
    deleteBook(bookId: string): Promise<void>;
    getBookById(bookId: string): Promise<Book>;
    getBookList(): Promise<Book[]>;
}

// Implementa la interfaz del servicio de libros
const bookService: BookService = {
    async insertBook(createBookCommand: CreateBookCommand): Promise<void> {
        try {
            await axios.post<void>(baseURL, createBookCommand);
        } catch (error) {
            handleError(error);
        }
    },

    async updateBook(bookId: string, updateBookCommand: UpdateBookCommand): Promise<void> {
        try {
            await axios.put<void>(`${baseURL}/${bookId}`, updateBookCommand);
        } catch (error) {
            handleError(error);
        }
    },

    async deleteBook(bookId: string): Promise<void> {
        try {
            await axios.delete<void>(`${baseURL}${bookId}`);
        } catch (error) {
            handleError(error);
        }
    },

    async getBookById(bookId: string): Promise<Book> {
        try {
            const response: AxiosResponse<Book> = await axios.get<Book>(`${baseURL}/${bookId}`);
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },

    async getBookList(): Promise<any> {
        try {
            debugger
            const response: any = await axios.get<any>(baseURL);
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },
};

// Función para manejar errores de Axios
const handleError = (error: any): void => {
    if (error.response) {
        // El servidor respondió con un código de estado que no está en el rango 2xx
        console.error('Error de servidor:', error.response.data);
        throw new Error(error.response.data);
    } else if (error.request) {
        // La solicitud fue realizada pero no se recibió respuesta
        console.error('No se recibió respuesta del servidor:', error.request);
        throw new Error('No se recibió respuesta del servidor');
    } else {
        // Ocurrió un error durante la configuración de la solicitud
        console.error('Error al configurar la solicitud:', error.message);
        throw new Error(error.message);
    }
};

export default bookService;
