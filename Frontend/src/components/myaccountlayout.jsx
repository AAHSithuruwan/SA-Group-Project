import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar/Navbar';
import Footer from './footer/footer';
import MyAccountsidebar from './myaccountsidebar/myaccountsidebar';

const MyAccountLayout = () => {
  return (
    <div style={{ height: '100vh', display: 'flex', flexDirection: 'column' }}>
      {/* Navbar at the top */}
      <Navbar />

      {/* Main layout - sidebar and content */}
      <div style={{ display: 'flex', flex: 1 }}>
        
        {/* Sidebar */}
       <MyAccountsidebar/>

        {/* Scrollable Main Content with background color */}
        <main
          style={{
            flex: 1,
            padding: '20px',
            paddingTop: '100px',
            backgroundColor: '#e9e0e0',  // Background color for the Outlet section
            overflowY: 'auto',
            height: 'calc(120vh - 140px)', // Adjust height for header and footer
          }}
        >
          {/* Render the content based on the current route */}
          <Outlet />
        </main>
      </div>

      {/* Footer at the bottom */}
      <Footer />
    </div>
  );
};

export default MyAccountLayout;
