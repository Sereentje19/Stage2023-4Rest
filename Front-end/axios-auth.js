import axios from 'axios';

const instance = axios.create({
    baseURL: 'http://localhost:5080/'
})

export default instance;