import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';
import Sidebar from './Sidebar/Sidebar'; 

const SellerLayout = () => {
  return (
    <div>
      <Navbar />
      <div style={{ display: 'flex', paddingTop: '70px' }}>
        <Sidebar /> 
        <main style={{ flex: 1, padding: '20px' ,background:'#e9e0e0' }}>
          <Outlet /> {/*  render child routes-  Dashboard, SellerDetails... */}
        </main>
      </div>
      <Footer />
    </div>
  );
};

export default SellerLayout;
