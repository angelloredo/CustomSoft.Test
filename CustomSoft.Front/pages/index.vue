<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="12">
    
      <v-card>
   

        <MyComponent/>
        
      
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
import MyComponent from '~/components/BookComponents/book-list-component.vue'

export default {
  name: 'IndexPage',
  components: {
    MyComponent
  }
}
</script>
