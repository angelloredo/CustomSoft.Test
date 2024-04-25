import axios from 'axios';
import { Book } from '~/viewModel/BookViewModel';
import * as XLSX from 'xlsx';
const baseURL = 'http://localhost:33755/api/book'

    
// Agregar el token Bearer a las cabeceras
const headers = {
    Authorization: 'Bearer your-token-here'
}


// Define la interfaz para el comando de creación de libros
interface CreateBookCommand {
    Title: string;
    FileDirection?: string;
}

// Define la interfaz para el comando de actualización de libros
interface UpdateBookCommand {
    BookId: string;
    Title?: string;
    FileDirection?: string;
}

// Define la interfaz para el servicio de libros
interface BookService {
    insertBook(createBookCommand: CreateBookCommand): Promise<void>;
    updateBook(bookId: string, updateBookCommand: UpdateBookCommand): Promise<void>;
    deleteBook(bookId: string): Promise<void>;
    getBookById(bookId: string): Promise<any>;
    getBookList(): Promise<Book[]>;
    uploadFile(bookId: string, file: File): Promise<any>;
    downloadFile(fileName: string): Promise<void>;
    exportToExcel(data: any, fileName, sheetName: string = 'Sheet1'): void;
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
            await axios.put<void>(`${baseURL}`, updateBookCommand);
        } catch (error) {
            handleError(error);
        }
    },

    async deleteBook(bookId: string): Promise<void> {
        try {
            await axios.delete<void>(`${baseURL}/${bookId}`);
        } catch (error) {
            handleError(error);
        }
    },

    async getBookById(bookId: string): Promise<any> {

        try {
            const response: any = await axios.get<Book>(`${baseURL}/${bookId}`);
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },

    async getBookList(): Promise<any> {
        try {

            const response: any = await axios.get<any>(baseURL, { headers: { 'X-Api-Key': 'my-secret-api-key' } });
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },
    async uploadFile(bookId: string, file: File): Promise<any> {
        try {
            debugger
            const formData = new FormData();
            formData.append('file', file);

            const config = {
                headers: {
                    'content-type': 'multipart/form-data'
                }
            };

            const response: any = await axios.post(`${baseURL}/upload/${bookId}`, formData, config);
            return response.data;
        } catch (error) {
            handleError(error);
        }
    },
    async downloadFile(fileName: string): Promise<void> {
        try {
            const response = await axios.get(`${baseURL}/download/${fileName}`, {
                responseType: 'blob'
            });

            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } catch (error) {
            handleError(error);
        }
    },
    exportToExcel(data, fileName, title, sheetName = 'Libros') {
        const worksheet = XLSX.utils.json_to_sheet(data);



        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);
        XLSX.writeFile(workbook, fileName);
    }

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
