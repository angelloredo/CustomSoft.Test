<template>
    <div>
        <h2>Listado de Libros</h2>
        <!-- Componente de Agregar Libro -->
        <v-dialog id="AgregarLibro" v-model="showAddForm" max-width="500"
            v-on:close="showAddForm = false; selectedBookId = ''">
            <template v-slot:activator="{ on }">
                <v-btn color="primary" dark v-on="on">Agregar Libro</v-btn>
            </template>
            <v-card>
                <v-card-title>Agregar Nuevo Libro</v-card-title>
                <v-card-text>
                    <AddBookForm v-if="showAddForm" :selectedId="selectedBookId" @bookSaved="onBookAdded"
                        @cancel="showAddForm = false" />
                </v-card-text>
            </v-card>
        </v-dialog>

        <v-dialog id="details" v-model="showDetailsDialog" max-width="500" v-on:close="showDetailsDialog = false">
            <v-card>
                <v-card-text>
                    <BookDetailsDialog :bookId="selectedBookId" v-if="showDetailsDialog"
                        @close="showDetailsDialog = false" />
                </v-card-text>
            </v-card>
        </v-dialog>

        <!-- Componente de Editar Libro -->
        <v-dialog id="editarLibro" v-model="showEditForm" max-width="500" v-on:close="showEditForm = false">
            <template v-slot:activator="{ on }">
                <!-- No hay activador visible ya que el formulario se muestra automáticamente -->
            </template>
            <v-card>
                <v-card-title>Editar Libro</v-card-title>
                <v-card-text>
                    <AddBookForm v-if="showEditForm" :selectedId="selectedBookId" @bookSaved="onBookEdited"
                        @cancel="showEditForm = false" />
                </v-card-text>
            </v-card>
        </v-dialog>
        <v-btn @click="exportToExcel" color="green">Exportar <v-icon>mdi-file-excel</v-icon></v-btn>
        <v-text-field v-model="search" label="Buscar libros" width="300" prepend-icon="mdi-magnify"></v-text-field>
        <!-- Tabla de Libros -->
        <v-data-table :headers="headers" :items="books" item-key="BookId" :search="search">
            <template v-slot:item="{ item }">
                <tr>
                    <td>{{ item.Title }}</td>
                    <td>{{ item.PublicationDate }}</td>
                    <td>{{ item.AuthorName }}</td>
                    <td>
                        <v-icon @click="openDetailsDialog(item.BookId)">mdi-eye</v-icon>
                        <v-icon @click="deleteBook(item.BookId)">mdi-delete</v-icon>
                        <v-icon @click="editBook(item.BookId)">mdi-pencil</v-icon>
                        <v-icon :title="item.FileName" v-if="item.FileName !== null"
                            @click="downloadBook(item)">mdi-download</v-icon>
                    </td>
                </tr>
            </template>
        </v-data-table>



    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import AddBookForm from '~/components/BookComponents/add-edit-form-component.vue';
import BookDetailsDialog from '~/components/BookComponents/details-component.vue';
import { Book } from '~/viewModel/BookViewModel';
import BookService from '~/services/BookServices';

// En algún lugar donde tengas acceso al contexto


export default defineComponent({
    components: {
        AddBookForm,
        BookDetailsDialog,
    },
    data() {
        return {
            books: [] as Book[],
            headers: [
                { text: 'Título', value: 'Title' },
                { text: 'Fecha de Publicación', value: 'PublicationDate' },
                { text: 'Autor del Libro', value: 'AuthorName' },
                { text: 'Acciones', value: 'actions' },
            ],
            showAddForm: false,
            showDetailsDialog: false,
            showEditForm: false,
            selectedBookId: '',//Variable para seccion de Edicion o Detalles
            search: '', // Variable para el término de búsqueda,
            bookService: new BookService(null)
        };
    },
    methods: {
        downloadBook(book: Book) {
            this.bookService.downloadFile(book);
        },
        async loadBooks() {
            try {
                this.books = await this.bookService.getBookList();
            } catch (error) {
                console.error('Error al obtener el listado de libros:', error);
            }
        },
        async deleteBook(bookId: string) {
            try {
                await this.bookService.deleteBook(bookId);
                await this.loadBooks();
                console.log('Libro eliminado con éxito');
            } catch (error) {
                console.error('Error al eliminar el libro:', error);
            }
        },
        openDetailsDialog(bookId: string) {
            this.selectedBookId = bookId;
            this.showDetailsDialog = true;
        },
        editBook(bookId: string) {
            this.selectedBookId = bookId;
            this.showEditForm = true;
        },
        onBookAdded() {
            this.showAddForm = false;
            this.loadBooks();
        },
        onBookEdited() {
            this.showEditForm = false;
            this.loadBooks();
        },
        exportToExcel() {

            this.bookService.exportToExcel(this.books as any, 'libros.xlsx');
        }
    },
    async mounted() {
        this.bookService = new BookService(this);
        await this.loadBooks();
    },
    watch: {
        showAddForm() {
            this.selectedBookId = '';
        }
    }
});
</script>