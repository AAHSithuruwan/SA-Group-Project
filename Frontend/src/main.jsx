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
import Dashboard from './pages/dashboard/dashboard.jsx';

import SellerLayout from './components/sellerlayout.jsx';
import SellerRegistration from './pages/SellerRegistrationForm/SellerRegistrationform.jsx';
import SellerDetails from './pages/sellerdetails/sellerdetails.jsx';
import AddProduct from './pages/addproduct/addproduct.jsx';
import AuctionDetails from './pages/auctiondetails/auctiondetails.jsx';
import Notifications from './pages/notifications/notifications.jsx';

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
        path: "/item-details",
        element: <ItemDetails />, 
      },
      {
        path: "/SellerRegistrationform",
        element: <SellerRegistration />,
      },
    ],
  },
  {
    path: "/", 
    element: <SellerLayout />, 
    children: [
      {
        path: "dashboard",
        element: <Dashboard />, 
      },

      {
        path: "sellerdetails",
        element: <SellerDetails />, 
      },
      
      {
        path: "addproduct",
        element: <AddProduct/>, 
      },
      {
        path: "auctiondetails",
        element: <AuctionDetails/>, 
      },
      
      {
        path: "notifications",
        element: <Notifications/>, 
      },
      
      
    ],
  },
]);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} /> 
  </StrictMode>
);
