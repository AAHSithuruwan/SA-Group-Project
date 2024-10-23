import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';

const RootLayout = () => {
  return (
    <div>
      <Navbar />
      <main
          style={{
            flex: 1,
           
           paddingTop: '25px',
            backgroundColor: '#e9e0e0',  
         
          }}
        >
        <Outlet /> 
      </main>
      <Footer />
    </div>
  );
};

export default RootLayout;
