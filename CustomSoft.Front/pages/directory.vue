<template>
  <v-row>
    <v-col class="text-center">
    
    <BookDirectory></BookDirectory>
    </v-col>
  </v-row>
</template>

<script>
import BookDirectory from '~/components/BookComponents/book-directory-component.vue' 
export default {
  name: 'DirectoryPage',
  components: {
    BookDirectory
  }
}
</script>
