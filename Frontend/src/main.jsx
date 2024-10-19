import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import RootLayout from './components/RootLayout'; 
import Signup from './pages/signup/signup.jsx';
import SignIn from './pages/signin/signin.jsx';
import Categories from './pages/categories/categories.jsx';
import Hero from './components/Hero/Hero.jsx'; 
import CategoryItems from './pages/categoryItems/categoryItems.jsx';
import ItemDetails from './pages/itemDetails/itemDetails.jsx'; 
import Dashboard from './pages/dashboard/dashboard.jsx'
import SellerRegistration from './pages/SellerRegistrationForm/SellerRegistrationform.jsx';


const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />, 

    children: [
      {
        path: "/",
        element: <Hero />, 
      },
      {
        path: "/dashboard",
        element: < Dashboard/>, 
      },

      {
        path: "/SellerRegistrationform",
        element:<SellerRegistration/>,
        
      },

      {
        path: "/signin",
        element: <SignIn />, 
      },
      {
        path: "/categories",
        element: <Categories />, 
      },
      {
        path: "/categories/items",
        element: <CategoryItems />, 
      },
      {
        path: "/item-details",  // Route for the item details page
        element: <ItemDetails />, 
      },
    ],
  },
]);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} /> 
  </StrictMode>
);
