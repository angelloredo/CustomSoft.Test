<template>
    <v-form ref="form" v-model="valid" lazy-validation>
        <br>
        <h2>Detalles del Libro</h2>
        <div v-if="isLoading">Cargando...</div>
        <div v-else-if="error">Error: {{ error }}</div>
        <div v-else>
            <p><strong>Título:</strong> {{ book?.Title }}</p>
            <p><strong>ID del Autor del Libro:</strong> {{ book?.BookAuthorGuid }}</p>
            <p><strong>Nombre del Autor:</strong> {{ book?.AuthorName }}</p>
            <p><strong>Apellido del Autor:</strong> {{ book?.AuthorLastName }}</p>

            <p><strong>Fecha de Publicación:</strong> {{ book?.PublicationDate }} </p>
            <p v-if="book?.FileName"><strong>Documento:</strong> {{ book?.FileName }} <v-icon :title="book.FileName"
                    @click="downloadBook(book)">mdi-download</v-icon> </p>
        </div>
    </v-form>

</template>

<script lang="ts">
import { defineComponent, PropType, ref } from 'vue';
import BookService from '~/services/BookServices';
import { Book, BookViewModel } from '~/viewModel/BookViewModel';


export default defineComponent({
    props: {
        bookId: {
            type: String as PropType<string>,
            required: true

        },
    },
    data() {
        return {
            book: null as Book | null,
            isLoading: false,
            error: '',
            valid: false,
            bookService: new BookService(null)
        };
    },
    async mounted() {
        this.bookService = new BookService(this);
        await this.fetchBook();
    },
    methods: {
        downloadBook(book: Book | null) {
            this.bookService.downloadFile(book as Book);
        },
        resetForm() {
            // Reseteamos el formulario
            (this.$refs.form as any).reset();
            this.book = new BookViewModel();
            this.valid = false;
        },
        async fetchBook() {

            this.isLoading = true;
            try {
                this.book = await this.bookService.getBookById(this.bookId);
            } catch (err) {
                this.error = 'No se pudo cargar el libro.';
                console.error('Error al obtener el libro:', err);
            } finally {
                this.isLoading = false;
            }
        },
    },
});
</script>