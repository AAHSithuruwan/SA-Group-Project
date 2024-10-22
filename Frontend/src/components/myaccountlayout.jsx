import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';
import MyAccountsidebar from './myaccountsidebar/myaccountsidebar';

const MyAccountLayout = () => {
  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column' }}>
      
      <Navbar />

     
      <div style={{ display: 'flex', flex: 1 }}>
        
        
       <MyAccountsidebar/>

        <main
          style={{
            flex: 1,
            padding: '20px',
            paddingTop: '100px',
            backgroundColor: '#e9e0e0',  
            overflowY: 'auto',
            height: 'calc(120vh - 140px)', 
          }}
        >
        
          <Outlet />
        </main>
      </div>

      <Footer />
    </div>
  );
};

export default MyAccountLayout;
