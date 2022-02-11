export default [

    {
        header: 'Sayfa İşlemleri',
        resources: ['Otherpage' ? 'Otherpage' : 'Basepage' ? 'Basepage' : 'null'],
        action: 'read',
    },
    {
        title: 'Temel Sayfalar',
        icon: 'FileIcon',
        children: [
            {
                title: 'Tüm Temel Sayfalar',
                route: { name: 'pages-basepage-list' },
                resource: 'Basepage',
                action: 'read',
            },
            {
                title: 'Yeni Ekle',
                route: { name: 'apps-invoice-preview', params: { id: 4987 } },
                resource: 'Basepage',
                action: 'create',
            },
        ],
    },
    {
        title: 'Sayfalar',
        icon: 'FileIcon',
        children: [
            {
                title: 'Tüm Sayfalar',
                route: { name: 'pages-page-list' },
                resource: 'Otherpage',
                action: 'read',
            },
            {
                title: 'Yeni Ekle',
                route: { name: 'pages-page-add' },
                resource: 'Otherpage',
                action: 'create',
            },
        ],
    },
    {
        header: 'Blog',
        resource: 'Article' ? 'Article' : 'Category' ? 'Category' : 'Tag' ? 'Tag' : 'null',
        action: 'read',
    },
    {
        title: 'Makaleler',
        icon: 'FileTextIcon',
        children: [
            {
                title: 'Tüm Makaleler',
                route: { name: 'pages-article-list' },
                resource: 'Article',
                action: 'read',
            },
            {
                title: 'Yeni Ekle',
                route: { name: 'pages-article-add' },
                resource: 'Article',
                action: 'create',
            },
        ],
    },
    {
        title: 'Kategoriler',
        icon: 'CopyIcon',
        route: { name: 'pages-category-list' },
        resource: 'Category',
        action: 'read',

    },
    {
        title: 'Etiketler',
        icon: 'TagIcon',
        route: { name: 'pages-tag-list' },
        resource: 'Tag',
        action: 'read',
    },
    {
        header: 'Araçlar',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        title: 'Link Yönlendirmeleri',
        icon: 'RepeatIcon',
        route: 'pages-urlredirect-list',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        header: 'Kullanıcı İşlemleri',
        resource: 'User',
        action: 'read',
    },
    {
        title: 'Kullanıcılar',
        icon: 'UsersIcon',
        children: [
            {
                title: 'Tüm Kullanıcılar',
                route: { name: 'pages-user-list' },
                resource: 'User',
                action: 'read',
            },
            {
                title: 'Yeni Ekle',
                route: { name: 'pages-user-add' },
                resource: 'User',
                action: 'create',
            },
        ],
    },
]
