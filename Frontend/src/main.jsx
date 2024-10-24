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

import SellerLayout from './components/sellerlayout.jsx';
import SellerRegistration from './pages/SellerRegistrationForm/SellerRegistrationform.jsx';
import SellerDetails from './pages/sellerdetails/sellerdetails.jsx';
import CreateAuction from './pages/createAuction/createAuction.jsx';
import SellerAuctionList from './pages/sellerAuctionList/sellerAuctionList.jsx';
import SellerAuctionDetails from './pages/sellerAuctionDetails/sellerAuctionDetails.jsx';
import UpdateAuction from './pages/updateAuction/updateAuction.jsx';
import SellerAuctionAllBids from './pages/sellerAuctionAllBids/sellerAuctionAllBids.jsx';

import AdminLayout from './components/adminlayout.jsx';
import AdminAuctionList from './pages/adminAuctionList/adminAuctionList.jsx';
import AdminAuctionDetails from './pages/adminauctiondetails/adminauctiondetails.jsx';
import AdminAuctionAllBids from './pages/adminAuctionAllBids/adminAuctionAllBids.jsx';
import AdminCategoryList from './pages/adminCategoryList/adminCategoryList.jsx';
import AddCategory from './pages/addCategory/addCategory.jsx';
import UpdateCategory from './pages/updateCategory/updateCategory.jsx';

// Import new components for the Account, Register, and Payment pages
import MyAccountLayout from './components/myaccountlayout.jsx';
import MyAccount from './pages/myaccount/myaccount.jsx';
import ChangeEmail from './pages/changeemail/changeemail.jsx';
import ChangePassword from './pages/changepassword/changepassword.jsx';
import UserAuctionList from './pages/userAuctionList/userAuctionList.jsx';
import UserAuctionDetails from './pages/userAuctionDetails/userAuctionDetails.jsx';
import UserAuctionAllBids from './pages/userAuctionAllBids/userAuctionAllBids.jsx';
import UserAuctionUserBids from './pages/userAuctionUserBids/userAuctionUserBids.jsx';
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
        path: "/item-details/payment",
        element: <Payment />
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
        path: "sellerauctionlist/sellerauctiondetails",
        element: <SellerAuctionDetails/>
      },

      {
        path: "sellerauctionlist/sellerauctiondetails/updateauction",
        element: <UpdateAuction/>
      },

      {
        path: "sellerauctionlist/sellerauctiondetails/sellerAuctionAllBids",
        element: <SellerAuctionAllBids/>
      },
      
      
    ],
  },

  {
    path: "/", 
    element: <AdminLayout />, 
    children: [

      {
        path: "adminauctionlist",
        element: <AdminAuctionList />, 
      },
      
      {
        path: "adminauctionlist/adminauctiondetails",
        element: <AdminAuctionDetails />
      },

      {
        path: "adminauctionlist/adminauctiondetails/adminauctionallbids",
        element: <AdminAuctionAllBids />
      },

      {
        path: "admincategorylist",
        element: <AdminCategoryList />
      },

      {
        path: "admincategorylist/addcategory",
        element: <AddCategory />
      },

      {
        path: "admincategorylist/updatecategory",
        element: <UpdateCategory />
      }
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
        path: "userauctionlist",
        element: <UserAuctionList/>, 
      },

      {
        path: "userauctionlist/userauctiondetails",
        element: <UserAuctionDetails/>, 
      },

      {
        path: "userauctionlist/userauctiondetails/userauctionallbids",
        element: <UserAuctionAllBids/>, 
      },

      {
        path: "userauctionlist/userauctiondetails/userauctionuserbids",
        element: <UserAuctionUserBids/>, 
      },
      
      {
        path: "myaccount/changeemail",
        element: <ChangeEmail/>, 
      },
      
      {
        path: "myaccount/changepassword",
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
