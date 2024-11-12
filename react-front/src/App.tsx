'use client'

import { useState, useEffect } from 'react'
import { PlusIcon, MinusIcon } from 'lucide-react'

interface Network {
  id: string
  name: string
  ipAddress: string
  isConnected: boolean
}

export default function App() {
  const [networks, setNetworks] = useState<Network[]>([])
  const [newNetwork, setNewNetwork] = useState({ name: '', ipAddress: '' })
  const [isAddingNetwork, setIsAddingNetwork] = useState(false)

  useEffect(() => {
    // Fetch networks from API
    // For demonstration, we'll use dummy data
    setNetworks([
      { id: '1', name: 'Network 1', ipAddress: '192.168.1.1', isConnected: true },
      { id: '2', name: 'Network 2', ipAddress: '10.0.0.1', isConnected: false },
    ])
  }, [])

  const addNetwork = () => {
    if (newNetwork.name && newNetwork.ipAddress) {
      const network: Network = {
        id: Date.now().toString(),
        ...newNetwork,
        isConnected: false
      }
      setNetworks([...networks, network])
      setNewNetwork({ name: '', ipAddress: '' })
      setIsAddingNetwork(false)
    }
  }

  const removeNetwork = (id: string) => {
    setNetworks(networks.filter(network => network.id !== id))
  }

  const updateNetworkStatus = (id: string, isConnected: boolean) => {
    setNetworks(networks.map(network => 
      network.id === id ? { ...network, isConnected } : network
    ))
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-5xl mx-auto">
        <h1 className="text-3xl font-extrabold text-gray-900 text-center mb-10">Network Management</h1>
        
        <div className="bg-white shadow-xl rounded-lg overflow-hidden mb-8">
          <div className="p-6">
            <h2 className="text-xl font-semibold text-gray-900 mb-4">Networks</h2>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">IP Address</th>
                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                    <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {networks.map((network) => (
                    <tr key={network.id} className="hover:bg-gray-50 transition-colors duration-200">
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{network.name}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{network.ipAddress}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                        <label className="flex items-center cursor-pointer">
                          <div className="relative">
                            <input
                              type="checkbox"
                              className="sr-only"
                              checked={network.isConnected}
                              onChange={(e) => updateNetworkStatus(network.id, e.target.checked)}
                            />
                            <div className="w-10 h-4 bg-gray-400 rounded-full shadow-inner"></div>
                            <div className={`absolute w-6 h-6 bg-white rounded-full shadow -left-1 -top-1 transition-all duration-300 ease-in-out ${network.isConnected ? 'transform translate-x-full bg-green-500' : 'bg-gray-200'}`}></div>
                          </div>
                          <div className="ml-3 text-sm font-medium text-gray-900">
                            {network.isConnected ? 'Connected' : 'Disconnected'}
                          </div>
                        </label>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                        <button
                          onClick={() => removeNetwork(network.id)}
                          className="text-red-600 hover:text-red-900 transition-colors duration-200"
                        >
                          Remove
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div className="bg-white shadow-xl rounded-lg overflow-hidden">
          <div className="p-6">
            <h2 className="text-xl font-semibold text-gray-900 mb-4">Add New Network</h2>
            {isAddingNetwork ? (
              <div className="space-y-4">
                <input
                  className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors duration-200"
                  placeholder="Network Name"
                  value={newNetwork.name}
                  onChange={(e) => setNewNetwork({...newNetwork, name: e.target.value})}
                />
                <input
                  className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors duration-200"
                  placeholder="IP Address"
                  value={newNetwork.ipAddress}
                  onChange={(e) => setNewNetwork({...newNetwork, ipAddress: e.target.value})}
                />
                <div className="flex space-x-4">
                  <button
                    className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors duration-200"
                    onClick={addNetwork}
                  >
                    Add Network
                  </button>
                  <button
                    className="flex-1 px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 transition-colors duration-200"
                    onClick={() => setIsAddingNetwork(false)}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            ) : (
              <button
                className="w-full px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors duration-200 flex items-center justify-center"
                onClick={() => setIsAddingNetwork(true)}
              >
                <PlusIcon className="w-5 h-5 mr-2" />
                Add New Network
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
