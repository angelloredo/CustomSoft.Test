<template>
    <v-form ref="form2" v-model="valid">
        <v-text-field v-model="book.Title" label="Título" required
            :rules="[v => !!v || 'El título es requerido']"></v-text-field>

        <v-select v-model="book.BookAuthorGuid" :items="authors" item-text="name" item-value="id"
            label="Autor del Libro" required :rules="[v => !!v || 'El autor del libro es requerido']"></v-select>

        <v-text-field v-model="book.PublicationDate" label="Fecha de Publicación" type="date" >

        </v-text-field>
        <!-- <v-text-field v-model="book.BookAuthorGuid" label="ID del Autor del Libro" required
            :rules="[v => !!v || 'El ID del autor es requerido']"></v-text-field> -->


        <!-- Componente v-file-input -->
        <v-file-input v-if="selectedId" v-model="file" label="Seleccionar archivo"
            accept="application/pdf"></v-file-input>

        <v-btn @click="saveBook" :disabled="!valid" color="primary">{{ buttonText }}</v-btn>
        <v-btn @click="resetForm" color="secondary" v-if="!selectedId">Resetear</v-btn>
        <v-btn @click="resetForm; $emit('cancel');" color="warning">Cancelar</v-btn>
    </v-form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { BookViewModel } from '../../viewModel/BookViewModel';
import BookService from '~/services/BookServices';

export default defineComponent({
    props: {
        selectedId: String,
    },
    data() {
        return {
            book:  BookViewModel.createEmpty(),
            valid: false,
            enabledAddButton: false,
            file: null,
            bookService: new BookService(null),
            authors: [] // Agrega esta línea
        };
    },
    computed: {
        buttonText(): string {
            return this.selectedId ? 'Actualizar' : 'Agregar';
        },
    },
    async mounted() {
        this.bookService = new BookService(this);
        var authors: any = await this.bookService.getAuthorListAsync();
        this.authors = authors.map((x: any) => {
            return {
                id: x.BookAuthorGuid,
                name: x.Name
            }
        })
        console.log("Autopres", authors)

        if (this.selectedId) {
            try {
                this.book = await this.bookService.getBookById(this.selectedId);
            } catch (error) {
                console.error('Error al cargar el libro:', error);
            }
        } else {
            this.book = BookViewModel.createEmpty();
        }
        console.log('Formulario válido:', this.valid);
        this.valid = false;
        this.$watch('valid', () => {
            // Log para verificar si 'valid' se actualiza correctamente
            console.log('Formulario válido:', this.valid);
        });
    },
    methods: {

        async saveBook() {
            try {
                if (this.selectedId) {
                    
                    await this.bookService.updateBook(this.selectedId, (this.book as any));

                    // Agregar el archivo si se proporciona
                    if (this.file) {
                        await this.bookService.uploadFile(this.book.BookId, this.file as any);
                        console.log('Archivo seleccionado:', this.file);
                    }
                    console.log('Libro actualizado con éxito');
                } else {

                    await this.bookService.insertBook(this.book);
                    console.log('Libro agregado con éxito');
                }
                this.$emit('bookSaved');
            } catch (error) {
                console.error('Error al guardar el libro:', error);
            }
        },
        resetForm() {
            (this.$refs.form2 as any).reset();
            this.book = BookViewModel.createEmpty();
            this.valid = false;
            this.enabledAddButton = false;
        },
    },
});
</script>