import axios from 'axios';

// Crear un objeto para contener tus servicios
const bookService = {};

// Configurar la URL base de tu API
const baseURL = 'http://localhost:33755/api/book';

// Función para manejar errores de Axios
const handleError = (error) => {
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

// Servicio para insertar un libro
bookService.insertBook = async (createBookCommand) => {
  try {
    const response = await axios.post(`${baseURL}`, createBookCommand);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// Servicio para actualizar un libro
bookService.updateBook = async (bookId, updateBookCommand) => {
  try {
    const response = await axios.put(`${baseURL}/${bookId}`, updateBookCommand);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// Servicio para eliminar un libro
bookService.deleteBook = async (bookId) => {
  try {
    const response = await axios.delete(`${baseURL}/${bookId}`);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// Servicio para obtener un libro por su ID
bookService.getBookById = async (bookId) => {
  try {
    const response = await axios.get(`${baseURL}/${bookId}`);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// Servicio para obtener la lista de libros
bookService.getBookList = async () => {
  try {
    const response = await axios.get(`${baseURL}`);
    return response.data;
  } catch (error) {
    handleError(error);
  }
};

// Exportar el objeto de servicios
export default bookService;