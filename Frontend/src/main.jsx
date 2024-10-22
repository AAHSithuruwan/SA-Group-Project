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


import SellerLayout from './components/sellerlayout.jsx';
import Dashboard from './pages/dashboard/dashboard.jsx';
import SellerDetails from './pages/sellerdetails/sellerdetails.jsx';
import AddProduct from './pages/addproduct/addproduct.jsx';             //create aution page
import AuctionDetails from './pages/auctiondetails/auctiondetails.jsx';
import SellerAuctionitemdetails from './pages/sellerauctionitemdetails/sellerauctionitemdetails.jsx';
import Notifications from './pages/notifications/notifications.jsx';

import AdminLayout from './components/adminlayout.jsx';
import AdminDashboard from './pages/admindashboard/admindashboard.jsx';
import AdminAuctionDetails from './pages/adminauctiondetails/adminauctiondetails.jsx';
import AdminAuctionItemDetails from './pages/adminauctionitemdetails/adminauctionitemdetails.jsx';
import CategoryDetails from './pages/categorydetails/categorydetails.jsx';
import AddNewCategory from './pages/addnewcategory/addnewcategory.jsx';


// Import new components for the Account, Register, and Payment pages
import MyAccountLayout from './components/myaccountlayout.jsx';
import MyAccount from './pages/myaccount/myaccount.jsx';
import BidHistory from './pages/bidhistory/bidhistory.jsx';
import ChangeEmail from './pages/changeemail/changeemail.jsx';
import ChangePassword from './pages/changepassword/changepassword.jsx';



import SellerRegistration from './pages/SellerRegistrationForm/Sellerregistrationform.jsx';
import Payment from './pages/payment/payment.jsx';



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


      // Add new routes here for  SellerRegistration, and Payment
     
      {
        path: "/sellerregistration",
        element: <SellerRegistration />, 
      },
      {
        path: "/payment",
        element: <Payment />, 
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
        element: <AddProduct/>,       //create aution page
      },
      {
        path: "auctiondetails",
        element: <AuctionDetails/>, 
      },

      {
        path: "sellerauctionitemdetails/:id",  
        element: <SellerAuctionitemdetails />,
      },
      
      {
        path: "notifications",
        element: <Notifications/>, 
      },
      
      
    ],
  },


  {
    path: "/", 
    element: <AdminLayout />, 

    children: [
      {
        path: "admindashboard",
        element: <AdminDashboard />, 
      },

      {
        path: "adminauctiondetails",
        element: <AdminAuctionDetails/>, 
      },

      {
        path: "adminauctionitemdetails/:id",  
        element: <AdminAuctionItemDetails />,
      },

      {
        path: "categorydetails",
        element: <CategoryDetails />, 
      },

      {
        path: "addnewcategory",
        element: <AddNewCategory />, 
      },


     
      
    ],
  },

  {
    path: "/", 
    element: <MyAccountLayout/>, 

    children: [
      {
        path: "myaccount",
        element: <MyAccount/>, 
      },


      {
        path: "bidhistory",
        element: <BidHistory/>, 
      },
      
      {
        path: "changeemail",
        element: <ChangeEmail/>, 
      },
      
      {
        path: "changepassword",
        element: <ChangePassword/>, 
      },

    
     
      
    ],
  },



]);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} /> 
  </StrictMode>
);
