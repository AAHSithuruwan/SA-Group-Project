import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import RootLayout from './components/RootLayout'; 
import SignUp from './pages/signup/signup.jsx';
import SignIn from './pages/signin/signin.jsx';
import Categories from './pages/categories/categories.jsx';
import Hero from './components/Hero/Hero.jsx'; 
import CategoryItems from './pages/categoryItems/categoryItems.jsx';
import ItemDetails from './pages/itemDetails/itemDetails.jsx'; 
import SellerDashboard from './pages/sellerDashboard/sellerDashboard.jsx';

import SellerLayout from './components/sellerlayout.jsx';
import SellerRegistration from './pages/SellerRegistrationForm/SellerRegistrationform.jsx';
import SellerDetails from './pages/sellerdetails/sellerdetails.jsx';
import CreateAuction from './pages/createAuction/createAuction.jsx';
import SellerAuctionList from './pages/sellerAuctionList/sellerAuctionList.jsx';
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
        path: "/signup",
        element: <SignUp />, 
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
        path: "sellerdashboard",
        element: <SellerDashboard />, 
      },

      {
        path: "sellerdetails",
        element: <SellerDetails />, 
      },
      
      {
        path: "createauction",
        element: <CreateAuction/>, 
      },
      {
        path: "sellerauctionlist",
        element: <SellerAuctionList/>, 
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
