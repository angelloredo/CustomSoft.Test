import axios from 'axios'

export default function ({ $axios }) {
    $axios.defaults.baseURL = 'http://tu-url-base.com';
}