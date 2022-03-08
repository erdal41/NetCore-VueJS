export default [

    {
        title: 'Media',
        icon: 'FolderIcon',
        route: 'pages-media-list',
        resource: 'Basepage',
        action: 'read',
    },
    {
        title: 'Base Pages',
        icon: 'FileIcon',
        children: [
            {
                title: 'All Base Pages',
                route: { name: 'pages-basepage-list' },
                resource: 'Basepage',
                action: 'read',
            },
            {
                title: 'Add New',
                route: { name: 'pages-basepage-add' },
                resource: 'Basepage',
                action: 'create',
            },
        ],
    },
    {
        title: 'Pages',
        icon: 'FileIcon',
        children: [
            {
                title: 'All Pages',
                route: { name: 'pages-page-list' },
                resource: 'Otherpage',
                action: 'read',
            },
            {
                title: 'Add New',
                route: { name: 'pages-page-add' },
                resource: 'Otherpage',
                action: 'create',
            },
        ],
    },
    {
        title: 'Articles',
        icon: 'FileTextIcon',
        children: [
            {
                title: 'All Articles',
                route: { name: 'pages-article-list' },
                resource: 'Article',
                action: 'read',
            },
            {
                title: 'Add New',
                route: { name: 'pages-article-add' },
                resource: 'Article',
                action: 'create',
            },
            {
                title: 'Categories',
                route: { name: 'pages-category-list' },
                resource: 'Category',
                action: 'read',
            },
            {
                title: 'Tags',
                route: { name: 'pages-tag-list' },
                resource: 'Tag',
                action: 'read',
            },
        ],
    },    
    {
        title: 'Url Redirects',
        icon: 'RepeatIcon',
        route: 'pages-urlredirect-list',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        title: 'All Users',
        icon: 'UsersIcon',
        children: [
            {
                title: 'All Users',
                route: { name: 'pages-user-list' },
                resource: 'User',
                action: 'read',
            },
            {
                title: 'Add New',
                route: { name: 'pages-user-add' },
                resource: 'User',
                action: 'create',
            },
        ],
    },
    {
        title: 'General Settings',
        icon: 'SettingsIcon',
        route: 'pages-general-settings',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        title: 'SEO Settings',
        icon: 'TrendingUpIcon',
        route: 'pages-seo-settings',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        title: 'Form Settings',
        icon: 'MailIcon',
        route: 'pages-form-settings',
        resource: 'Urlredirect',
        action: 'read',
    },
    {
        title: 'Widget Settings',
        icon: 'SlackIcon',
        route: 'pages-widget-settings',
        resource: 'Urlredirect',
        action: 'read',
    },
]
