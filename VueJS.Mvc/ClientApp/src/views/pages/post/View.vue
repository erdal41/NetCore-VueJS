<template>
    <h1>{{ pageTitle }}</h1>
</template>
<script>
    import axios from 'axios';
    import router from '@/router'

    export default {
        components: {
            router
        },
        data() {
            return {
                pageTitle: '',
            }
        },
        methods: {
            getData() {
                axios.get('/web/home/' + this.$route.params.postName)
                    .then((response) => {
                        if (response.data.UrlRedirectListDto.ResultStatus === 0) {
                            var fullUrl = window.location.origin + this.$router.currentRoute.fullPath;
                            var urlRedirect = response.data.UrlRedirectListDto.UrlRedirects.find(function (url) {
                                if (url.OldUrl == fullUrl) {
                                    var param = url.NewUrl.substring(url.NewUrl.lastIndexOf("/") + 1, url.NewUrl.length);
                                    router.replace({ name: 'pages-page-view', params: { postName: param } });
                                }
                            });

                            if (urlRedirect == undefined) {
                                if (response.data.PostDto != null && response.data.PostDto.ResultStatus === 0) {
                                    console.log(response.data);
                                    this.pageTitle = response.data.PostDto.Post.Title;
                                }
                            }
                        }
                        else {

                        }

                    })
                    .catch((error) => {
                        console.log(error);
                        console.log(error.request);
                    });
            },
        },
        watch: {
            $route(to, from) {
                this.getData();
            }
        },
        computed: {

        },
        mounted() {
            this.getData();
        }
    }
</script>

<style>
</style>