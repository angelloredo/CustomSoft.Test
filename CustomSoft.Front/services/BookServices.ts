import axios from 'axios';
import { Book, BookViewModel } from '~/viewModel/BookViewModel';
import * as XLSX from 'xlsx';

const baseURL = 'http://localhost:33755/api/book';
const baseURLAuthor = 'http://localhost:33755/api/Author';

interface CreateBookCommand {
    Title: string;
    FileDirection?: string;
}

interface UpdateBookCommand {
    BookId: string;
    Title?: string;
    FileDirection?: string;
    PublicationDate?: string | null

}

interface IBookService {
    insertBook(createBookCommand: CreateBookCommand): Promise<void>;
    updateBook(bookId: string, updateBookCommand: UpdateBookCommand): Promise<void>;
    deleteBook(bookId: string): Promise<void>;
    getBookById(bookId: string): Promise<any>;
    getBookList(): Promise<Book[]>;
    uploadFile(bookId: string, file: File): Promise<any>;
    downloadFile(book: Book): Promise<void>;
    exportToExcel(data: any, fileName: string): void;
    getAuthorListAsync(): Promise<any>;
}

class BookService implements IBookService {

    context: any;

    constructor(context: any) {
        this.context = context;
    }


    private handleError(error: any): void {
        let toastMessage = "Ha ocurrido un error. Por favor, inténtalo de nuevo más tarde.";

        if (error.response) {
            // El servidor respondió con un código de estado que no está en el rango 2xx
            console.error('Error de servidor:', error.response.data);
            if (error.response.data.errors.title) {
                toastMessage += error.response.data.errors.title.toString();
            }
            this.context.$toast.error(error.response.data.message || toastMessage); // Muestra mensaje toast de error
        } else if (error.request) {
            // La solicitud fue realizada pero no se recibió respuesta
            console.error('No se recibió respuesta del servidor:', error.request);
            this.context.$toast.error(toastMessage); // Muestra mensaje toast de error
        } else {
            // Ocurrió un error durante la configuración de la solicitud
            console.error('Error al configurar la solicitud:', error.message);
            this.context.$toast.error(toastMessage); // Muestra mensaje toast de error
        }
    }

    async insertBook(createBookCommand: CreateBookCommand): Promise<void> {
        try {
            await axios.post<void>(baseURL, createBookCommand);
        } catch (error) {
            this.handleError(error);
        }
    }

    async updateBook(bookId: string, updateBookCommand: UpdateBookCommand): Promise<void> {
        try {
            if (updateBookCommand.PublicationDate === "Sin publicar.") {
                updateBookCommand.PublicationDate = null;
            }
            await axios.put<void>(`${baseURL}`, updateBookCommand);
        } catch (error) {
            this.handleError(error);
        }
    }

    async deleteBook(bookId: string): Promise<void> {
        try {
            await axios.delete<void>(`${baseURL}/${bookId}`);
        } catch (error) {
            this.handleError(error);
        }
    }

    async getBookById(bookId: string): Promise<any> {
        try {
            const response = await axios.get<Book>(`${baseURL}/${bookId}`);
            return response.data;
        } catch (error) {
            this.handleError(error);
        }
    }

    async getBookList(): Promise<any> {
        try {
            const response = await axios.get<any>(baseURL, { headers: { 'X-Api-Key': 'my-secret-api-key' } });

            return response.data.map((x: any) => new BookViewModel(x.BookId,
                x.Title,
                x.PublicationDate,
                x.BookAuthorGuid,
                x.AuthorName,
                '',
                x.FileName,
            ));
            
        } catch (error) {
            this.handleError(error);
        }
    }

    async getAuthorListAsync(): Promise<any> {
        try {
            const response = await axios.get<any>(baseURLAuthor, { headers: { 'X-Api-Key': 'my-secret-api-key' } });
            return response.data;
        } catch (error) {
            this.handleError(error);
        }
    }

    async uploadFile(bookId: string, file: File): Promise<any> {
        try {
            const formData = new FormData();
            formData.append('file', file);
            const config = { headers: { 'content-type': 'multipart/form-data' } };
            await axios.post(`${baseURL}/upload/${bookId}`, formData, config);
        } catch (error) {
            this.handleError(error);
        }
    }

    async downloadFile(book: Book): Promise<void> {
        try {
            const response = await fetch(`${baseURL}/download/${book.BookId}`);
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `${book.Title.replace(/ /g, '_')}.pdf`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
        } catch (error) {
            console.error('Error al descargar el archivo:', error);
        }
    }

    exportToExcel(data: Book[], fileName: string): void {
        const mapData = data.map(x => {
            return {
                Titulo: x.Title,
                "Fecha de publicación": x.PublicationDate,
                Autor: x.AuthorName
            }
        });
        const worksheet = XLSX.utils.json_to_sheet(mapData);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Libros');
        XLSX.writeFile(workbook, fileName);
    }
}

export default BookService;
