import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';

const RootLayout = () => {
  return (
    <div>
      <Navbar />
      <main style={{ paddingTop: '25px' }}>
        <Outlet /> 
      </main>
      <Footer />
    </div>
  );
};

export default RootLayout;
