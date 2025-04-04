/* eslint-disable */

// @ts-nocheck

// noinspection JSUnusedGlobalSymbols

// This file was automatically generated by TanStack Router.
// You should NOT make any changes in this file as it will be overwritten.
// Additionally, you should also exclude this file from your linter and/or formatter to prevent it from being checked or modified.

// Import Routes

import { Route as rootRoute } from './routes/__root'
import { Route as AboutImport } from './routes/about'
import { Route as IndexImport } from './routes/index'
import { Route as ProfileIndexImport } from './routes/profile/index'
import { Route as BooksIndexImport } from './routes/books/index'
import { Route as AdminIndexImport } from './routes/admin/index'
import { Route as BooksBookIdImport } from './routes/books/$bookId'
import { Route as AuthRegisterImport } from './routes/auth/register'
import { Route as AuthLoginImport } from './routes/auth/login'
import { Route as ProfileEditUserIdImport } from './routes/profile/edit.$userId'
import { Route as AdminManageUsersImport } from './routes/admin/manage/users'
import { Route as AdminManageInteractionsImport } from './routes/admin/manage/interactions'
import { Route as AdminManageBooksImport } from './routes/admin/manage/books'
import { Route as AdminManageAuthorsImport } from './routes/admin/manage/authors'
import { Route as AdminManageArtistsImport } from './routes/admin/manage/artists'
import { Route as AdminCreateBookImport } from './routes/admin/create/book'
import { Route as AdminCreateAuthorImport } from './routes/admin/create/author'
import { Route as AdminCreateArtistImport } from './routes/admin/create/artist'
import { Route as AdminEditBookBookIdImport } from './routes/admin/edit/book.$bookId'
import { Route as AdminEditAuthorAuthorPersonIdImport } from './routes/admin/edit/author.$authorPersonId'
import { Route as AdminEditArtistArtistPersonIdImport } from './routes/admin/edit/artist.$artistPersonId'

// Create/Update Routes

const AboutRoute = AboutImport.update({
  id: '/about',
  path: '/about',
  getParentRoute: () => rootRoute,
} as any)

const IndexRoute = IndexImport.update({
  id: '/',
  path: '/',
  getParentRoute: () => rootRoute,
} as any)

const ProfileIndexRoute = ProfileIndexImport.update({
  id: '/profile/',
  path: '/profile/',
  getParentRoute: () => rootRoute,
} as any)

const BooksIndexRoute = BooksIndexImport.update({
  id: '/books/',
  path: '/books/',
  getParentRoute: () => rootRoute,
} as any)

const AdminIndexRoute = AdminIndexImport.update({
  id: '/admin/',
  path: '/admin/',
  getParentRoute: () => rootRoute,
} as any)

const BooksBookIdRoute = BooksBookIdImport.update({
  id: '/books/$bookId',
  path: '/books/$bookId',
  getParentRoute: () => rootRoute,
} as any)

const AuthRegisterRoute = AuthRegisterImport.update({
  id: '/auth/register',
  path: '/auth/register',
  getParentRoute: () => rootRoute,
} as any)

const AuthLoginRoute = AuthLoginImport.update({
  id: '/auth/login',
  path: '/auth/login',
  getParentRoute: () => rootRoute,
} as any)

const ProfileEditUserIdRoute = ProfileEditUserIdImport.update({
  id: '/profile/edit/$userId',
  path: '/profile/edit/$userId',
  getParentRoute: () => rootRoute,
} as any)

const AdminManageUsersRoute = AdminManageUsersImport.update({
  id: '/admin/manage/users',
  path: '/admin/manage/users',
  getParentRoute: () => rootRoute,
} as any)

const AdminManageInteractionsRoute = AdminManageInteractionsImport.update({
  id: '/admin/manage/interactions',
  path: '/admin/manage/interactions',
  getParentRoute: () => rootRoute,
} as any)

const AdminManageBooksRoute = AdminManageBooksImport.update({
  id: '/admin/manage/books',
  path: '/admin/manage/books',
  getParentRoute: () => rootRoute,
} as any)

const AdminManageAuthorsRoute = AdminManageAuthorsImport.update({
  id: '/admin/manage/authors',
  path: '/admin/manage/authors',
  getParentRoute: () => rootRoute,
} as any)

const AdminManageArtistsRoute = AdminManageArtistsImport.update({
  id: '/admin/manage/artists',
  path: '/admin/manage/artists',
  getParentRoute: () => rootRoute,
} as any)

const AdminCreateBookRoute = AdminCreateBookImport.update({
  id: '/admin/create/book',
  path: '/admin/create/book',
  getParentRoute: () => rootRoute,
} as any)

const AdminCreateAuthorRoute = AdminCreateAuthorImport.update({
  id: '/admin/create/author',
  path: '/admin/create/author',
  getParentRoute: () => rootRoute,
} as any)

const AdminCreateArtistRoute = AdminCreateArtistImport.update({
  id: '/admin/create/artist',
  path: '/admin/create/artist',
  getParentRoute: () => rootRoute,
} as any)

const AdminEditBookBookIdRoute = AdminEditBookBookIdImport.update({
  id: '/admin/edit/book/$bookId',
  path: '/admin/edit/book/$bookId',
  getParentRoute: () => rootRoute,
} as any)

const AdminEditAuthorAuthorPersonIdRoute =
  AdminEditAuthorAuthorPersonIdImport.update({
    id: '/admin/edit/author/$authorPersonId',
    path: '/admin/edit/author/$authorPersonId',
    getParentRoute: () => rootRoute,
  } as any)

const AdminEditArtistArtistPersonIdRoute =
  AdminEditArtistArtistPersonIdImport.update({
    id: '/admin/edit/artist/$artistPersonId',
    path: '/admin/edit/artist/$artistPersonId',
    getParentRoute: () => rootRoute,
  } as any)

// Populate the FileRoutesByPath interface

declare module '@tanstack/react-router' {
  interface FileRoutesByPath {
    '/': {
      id: '/'
      path: '/'
      fullPath: '/'
      preLoaderRoute: typeof IndexImport
      parentRoute: typeof rootRoute
    }
    '/about': {
      id: '/about'
      path: '/about'
      fullPath: '/about'
      preLoaderRoute: typeof AboutImport
      parentRoute: typeof rootRoute
    }
    '/auth/login': {
      id: '/auth/login'
      path: '/auth/login'
      fullPath: '/auth/login'
      preLoaderRoute: typeof AuthLoginImport
      parentRoute: typeof rootRoute
    }
    '/auth/register': {
      id: '/auth/register'
      path: '/auth/register'
      fullPath: '/auth/register'
      preLoaderRoute: typeof AuthRegisterImport
      parentRoute: typeof rootRoute
    }
    '/books/$bookId': {
      id: '/books/$bookId'
      path: '/books/$bookId'
      fullPath: '/books/$bookId'
      preLoaderRoute: typeof BooksBookIdImport
      parentRoute: typeof rootRoute
    }
    '/admin/': {
      id: '/admin/'
      path: '/admin'
      fullPath: '/admin'
      preLoaderRoute: typeof AdminIndexImport
      parentRoute: typeof rootRoute
    }
    '/books/': {
      id: '/books/'
      path: '/books'
      fullPath: '/books'
      preLoaderRoute: typeof BooksIndexImport
      parentRoute: typeof rootRoute
    }
    '/profile/': {
      id: '/profile/'
      path: '/profile'
      fullPath: '/profile'
      preLoaderRoute: typeof ProfileIndexImport
      parentRoute: typeof rootRoute
    }
    '/admin/create/artist': {
      id: '/admin/create/artist'
      path: '/admin/create/artist'
      fullPath: '/admin/create/artist'
      preLoaderRoute: typeof AdminCreateArtistImport
      parentRoute: typeof rootRoute
    }
    '/admin/create/author': {
      id: '/admin/create/author'
      path: '/admin/create/author'
      fullPath: '/admin/create/author'
      preLoaderRoute: typeof AdminCreateAuthorImport
      parentRoute: typeof rootRoute
    }
    '/admin/create/book': {
      id: '/admin/create/book'
      path: '/admin/create/book'
      fullPath: '/admin/create/book'
      preLoaderRoute: typeof AdminCreateBookImport
      parentRoute: typeof rootRoute
    }
    '/admin/manage/artists': {
      id: '/admin/manage/artists'
      path: '/admin/manage/artists'
      fullPath: '/admin/manage/artists'
      preLoaderRoute: typeof AdminManageArtistsImport
      parentRoute: typeof rootRoute
    }
    '/admin/manage/authors': {
      id: '/admin/manage/authors'
      path: '/admin/manage/authors'
      fullPath: '/admin/manage/authors'
      preLoaderRoute: typeof AdminManageAuthorsImport
      parentRoute: typeof rootRoute
    }
    '/admin/manage/books': {
      id: '/admin/manage/books'
      path: '/admin/manage/books'
      fullPath: '/admin/manage/books'
      preLoaderRoute: typeof AdminManageBooksImport
      parentRoute: typeof rootRoute
    }
    '/admin/manage/interactions': {
      id: '/admin/manage/interactions'
      path: '/admin/manage/interactions'
      fullPath: '/admin/manage/interactions'
      preLoaderRoute: typeof AdminManageInteractionsImport
      parentRoute: typeof rootRoute
    }
    '/admin/manage/users': {
      id: '/admin/manage/users'
      path: '/admin/manage/users'
      fullPath: '/admin/manage/users'
      preLoaderRoute: typeof AdminManageUsersImport
      parentRoute: typeof rootRoute
    }
    '/profile/edit/$userId': {
      id: '/profile/edit/$userId'
      path: '/profile/edit/$userId'
      fullPath: '/profile/edit/$userId'
      preLoaderRoute: typeof ProfileEditUserIdImport
      parentRoute: typeof rootRoute
    }
    '/admin/edit/artist/$artistPersonId': {
      id: '/admin/edit/artist/$artistPersonId'
      path: '/admin/edit/artist/$artistPersonId'
      fullPath: '/admin/edit/artist/$artistPersonId'
      preLoaderRoute: typeof AdminEditArtistArtistPersonIdImport
      parentRoute: typeof rootRoute
    }
    '/admin/edit/author/$authorPersonId': {
      id: '/admin/edit/author/$authorPersonId'
      path: '/admin/edit/author/$authorPersonId'
      fullPath: '/admin/edit/author/$authorPersonId'
      preLoaderRoute: typeof AdminEditAuthorAuthorPersonIdImport
      parentRoute: typeof rootRoute
    }
    '/admin/edit/book/$bookId': {
      id: '/admin/edit/book/$bookId'
      path: '/admin/edit/book/$bookId'
      fullPath: '/admin/edit/book/$bookId'
      preLoaderRoute: typeof AdminEditBookBookIdImport
      parentRoute: typeof rootRoute
    }
  }
}

// Create and export the route tree

export interface FileRoutesByFullPath {
  '/': typeof IndexRoute
  '/about': typeof AboutRoute
  '/auth/login': typeof AuthLoginRoute
  '/auth/register': typeof AuthRegisterRoute
  '/books/$bookId': typeof BooksBookIdRoute
  '/admin': typeof AdminIndexRoute
  '/books': typeof BooksIndexRoute
  '/profile': typeof ProfileIndexRoute
  '/admin/create/artist': typeof AdminCreateArtistRoute
  '/admin/create/author': typeof AdminCreateAuthorRoute
  '/admin/create/book': typeof AdminCreateBookRoute
  '/admin/manage/artists': typeof AdminManageArtistsRoute
  '/admin/manage/authors': typeof AdminManageAuthorsRoute
  '/admin/manage/books': typeof AdminManageBooksRoute
  '/admin/manage/interactions': typeof AdminManageInteractionsRoute
  '/admin/manage/users': typeof AdminManageUsersRoute
  '/profile/edit/$userId': typeof ProfileEditUserIdRoute
  '/admin/edit/artist/$artistPersonId': typeof AdminEditArtistArtistPersonIdRoute
  '/admin/edit/author/$authorPersonId': typeof AdminEditAuthorAuthorPersonIdRoute
  '/admin/edit/book/$bookId': typeof AdminEditBookBookIdRoute
}

export interface FileRoutesByTo {
  '/': typeof IndexRoute
  '/about': typeof AboutRoute
  '/auth/login': typeof AuthLoginRoute
  '/auth/register': typeof AuthRegisterRoute
  '/books/$bookId': typeof BooksBookIdRoute
  '/admin': typeof AdminIndexRoute
  '/books': typeof BooksIndexRoute
  '/profile': typeof ProfileIndexRoute
  '/admin/create/artist': typeof AdminCreateArtistRoute
  '/admin/create/author': typeof AdminCreateAuthorRoute
  '/admin/create/book': typeof AdminCreateBookRoute
  '/admin/manage/artists': typeof AdminManageArtistsRoute
  '/admin/manage/authors': typeof AdminManageAuthorsRoute
  '/admin/manage/books': typeof AdminManageBooksRoute
  '/admin/manage/interactions': typeof AdminManageInteractionsRoute
  '/admin/manage/users': typeof AdminManageUsersRoute
  '/profile/edit/$userId': typeof ProfileEditUserIdRoute
  '/admin/edit/artist/$artistPersonId': typeof AdminEditArtistArtistPersonIdRoute
  '/admin/edit/author/$authorPersonId': typeof AdminEditAuthorAuthorPersonIdRoute
  '/admin/edit/book/$bookId': typeof AdminEditBookBookIdRoute
}

export interface FileRoutesById {
  __root__: typeof rootRoute
  '/': typeof IndexRoute
  '/about': typeof AboutRoute
  '/auth/login': typeof AuthLoginRoute
  '/auth/register': typeof AuthRegisterRoute
  '/books/$bookId': typeof BooksBookIdRoute
  '/admin/': typeof AdminIndexRoute
  '/books/': typeof BooksIndexRoute
  '/profile/': typeof ProfileIndexRoute
  '/admin/create/artist': typeof AdminCreateArtistRoute
  '/admin/create/author': typeof AdminCreateAuthorRoute
  '/admin/create/book': typeof AdminCreateBookRoute
  '/admin/manage/artists': typeof AdminManageArtistsRoute
  '/admin/manage/authors': typeof AdminManageAuthorsRoute
  '/admin/manage/books': typeof AdminManageBooksRoute
  '/admin/manage/interactions': typeof AdminManageInteractionsRoute
  '/admin/manage/users': typeof AdminManageUsersRoute
  '/profile/edit/$userId': typeof ProfileEditUserIdRoute
  '/admin/edit/artist/$artistPersonId': typeof AdminEditArtistArtistPersonIdRoute
  '/admin/edit/author/$authorPersonId': typeof AdminEditAuthorAuthorPersonIdRoute
  '/admin/edit/book/$bookId': typeof AdminEditBookBookIdRoute
}

export interface FileRouteTypes {
  fileRoutesByFullPath: FileRoutesByFullPath
  fullPaths:
    | '/'
    | '/about'
    | '/auth/login'
    | '/auth/register'
    | '/books/$bookId'
    | '/admin'
    | '/books'
    | '/profile'
    | '/admin/create/artist'
    | '/admin/create/author'
    | '/admin/create/book'
    | '/admin/manage/artists'
    | '/admin/manage/authors'
    | '/admin/manage/books'
    | '/admin/manage/interactions'
    | '/admin/manage/users'
    | '/profile/edit/$userId'
    | '/admin/edit/artist/$artistPersonId'
    | '/admin/edit/author/$authorPersonId'
    | '/admin/edit/book/$bookId'
  fileRoutesByTo: FileRoutesByTo
  to:
    | '/'
    | '/about'
    | '/auth/login'
    | '/auth/register'
    | '/books/$bookId'
    | '/admin'
    | '/books'
    | '/profile'
    | '/admin/create/artist'
    | '/admin/create/author'
    | '/admin/create/book'
    | '/admin/manage/artists'
    | '/admin/manage/authors'
    | '/admin/manage/books'
    | '/admin/manage/interactions'
    | '/admin/manage/users'
    | '/profile/edit/$userId'
    | '/admin/edit/artist/$artistPersonId'
    | '/admin/edit/author/$authorPersonId'
    | '/admin/edit/book/$bookId'
  id:
    | '__root__'
    | '/'
    | '/about'
    | '/auth/login'
    | '/auth/register'
    | '/books/$bookId'
    | '/admin/'
    | '/books/'
    | '/profile/'
    | '/admin/create/artist'
    | '/admin/create/author'
    | '/admin/create/book'
    | '/admin/manage/artists'
    | '/admin/manage/authors'
    | '/admin/manage/books'
    | '/admin/manage/interactions'
    | '/admin/manage/users'
    | '/profile/edit/$userId'
    | '/admin/edit/artist/$artistPersonId'
    | '/admin/edit/author/$authorPersonId'
    | '/admin/edit/book/$bookId'
  fileRoutesById: FileRoutesById
}

export interface RootRouteChildren {
  IndexRoute: typeof IndexRoute
  AboutRoute: typeof AboutRoute
  AuthLoginRoute: typeof AuthLoginRoute
  AuthRegisterRoute: typeof AuthRegisterRoute
  BooksBookIdRoute: typeof BooksBookIdRoute
  AdminIndexRoute: typeof AdminIndexRoute
  BooksIndexRoute: typeof BooksIndexRoute
  ProfileIndexRoute: typeof ProfileIndexRoute
  AdminCreateArtistRoute: typeof AdminCreateArtistRoute
  AdminCreateAuthorRoute: typeof AdminCreateAuthorRoute
  AdminCreateBookRoute: typeof AdminCreateBookRoute
  AdminManageArtistsRoute: typeof AdminManageArtistsRoute
  AdminManageAuthorsRoute: typeof AdminManageAuthorsRoute
  AdminManageBooksRoute: typeof AdminManageBooksRoute
  AdminManageInteractionsRoute: typeof AdminManageInteractionsRoute
  AdminManageUsersRoute: typeof AdminManageUsersRoute
  ProfileEditUserIdRoute: typeof ProfileEditUserIdRoute
  AdminEditArtistArtistPersonIdRoute: typeof AdminEditArtistArtistPersonIdRoute
  AdminEditAuthorAuthorPersonIdRoute: typeof AdminEditAuthorAuthorPersonIdRoute
  AdminEditBookBookIdRoute: typeof AdminEditBookBookIdRoute
}

const rootRouteChildren: RootRouteChildren = {
  IndexRoute: IndexRoute,
  AboutRoute: AboutRoute,
  AuthLoginRoute: AuthLoginRoute,
  AuthRegisterRoute: AuthRegisterRoute,
  BooksBookIdRoute: BooksBookIdRoute,
  AdminIndexRoute: AdminIndexRoute,
  BooksIndexRoute: BooksIndexRoute,
  ProfileIndexRoute: ProfileIndexRoute,
  AdminCreateArtistRoute: AdminCreateArtistRoute,
  AdminCreateAuthorRoute: AdminCreateAuthorRoute,
  AdminCreateBookRoute: AdminCreateBookRoute,
  AdminManageArtistsRoute: AdminManageArtistsRoute,
  AdminManageAuthorsRoute: AdminManageAuthorsRoute,
  AdminManageBooksRoute: AdminManageBooksRoute,
  AdminManageInteractionsRoute: AdminManageInteractionsRoute,
  AdminManageUsersRoute: AdminManageUsersRoute,
  ProfileEditUserIdRoute: ProfileEditUserIdRoute,
  AdminEditArtistArtistPersonIdRoute: AdminEditArtistArtistPersonIdRoute,
  AdminEditAuthorAuthorPersonIdRoute: AdminEditAuthorAuthorPersonIdRoute,
  AdminEditBookBookIdRoute: AdminEditBookBookIdRoute,
}

export const routeTree = rootRoute
  ._addFileChildren(rootRouteChildren)
  ._addFileTypes<FileRouteTypes>()

/* ROUTE_MANIFEST_START
{
  "routes": {
    "__root__": {
      "filePath": "__root.tsx",
      "children": [
        "/",
        "/about",
        "/auth/login",
        "/auth/register",
        "/books/$bookId",
        "/admin/",
        "/books/",
        "/profile/",
        "/admin/create/artist",
        "/admin/create/author",
        "/admin/create/book",
        "/admin/manage/artists",
        "/admin/manage/authors",
        "/admin/manage/books",
        "/admin/manage/interactions",
        "/admin/manage/users",
        "/profile/edit/$userId",
        "/admin/edit/artist/$artistPersonId",
        "/admin/edit/author/$authorPersonId",
        "/admin/edit/book/$bookId"
      ]
    },
    "/": {
      "filePath": "index.tsx"
    },
    "/about": {
      "filePath": "about.tsx"
    },
    "/auth/login": {
      "filePath": "auth/login.tsx"
    },
    "/auth/register": {
      "filePath": "auth/register.tsx"
    },
    "/books/$bookId": {
      "filePath": "books/$bookId.tsx"
    },
    "/admin/": {
      "filePath": "admin/index.tsx"
    },
    "/books/": {
      "filePath": "books/index.tsx"
    },
    "/profile/": {
      "filePath": "profile/index.tsx"
    },
    "/admin/create/artist": {
      "filePath": "admin/create/artist.tsx"
    },
    "/admin/create/author": {
      "filePath": "admin/create/author.tsx"
    },
    "/admin/create/book": {
      "filePath": "admin/create/book.tsx"
    },
    "/admin/manage/artists": {
      "filePath": "admin/manage/artists.tsx"
    },
    "/admin/manage/authors": {
      "filePath": "admin/manage/authors.tsx"
    },
    "/admin/manage/books": {
      "filePath": "admin/manage/books.tsx"
    },
    "/admin/manage/interactions": {
      "filePath": "admin/manage/interactions.tsx"
    },
    "/admin/manage/users": {
      "filePath": "admin/manage/users.tsx"
    },
    "/profile/edit/$userId": {
      "filePath": "profile/edit.$userId.tsx"
    },
    "/admin/edit/artist/$artistPersonId": {
      "filePath": "admin/edit/artist.$artistPersonId.tsx"
    },
    "/admin/edit/author/$authorPersonId": {
      "filePath": "admin/edit/author.$authorPersonId.tsx"
    },
    "/admin/edit/book/$bookId": {
      "filePath": "admin/edit/book.$bookId.tsx"
    }
  }
}
ROUTE_MANIFEST_END */
